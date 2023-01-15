using System;
using System.Text.RegularExpressions;

namespace ET
{
    [FriendClass(typeof(Account))]
    public class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            Log.Error($"请求{request.AcccountName}");

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session?.DisConnect().Coroutine();
                return;
            }
            if (string.IsNullOrEmpty(request.AcccountName) || string.IsNullOrEmpty(request.PassWord))
            {
                response.Error = ErrorCode.ERR_LoginInfoError;
                reply();
                session?.DisConnect().Coroutine();
                return;
            }
            if (session.GetComponent<AccountsZone>() == null)
            {
                session.AddComponent<AccountsZone>();
            }
            if (session.GetComponent<RoleInfosZone>() == null)
            {
                session.AddComponent<RoleInfosZone>();
            }
            // if (Regex.IsMatch())
            // {
            //     
            // }
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount,request.AcccountName.Trim().GetHashCode()))
                {
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                        .Query<Account>(d => d.AccountName.Equals(request.AcccountName.Trim()));
                Account account = null;
                if (accountInfoList != null && accountInfoList.Count > 0)
                {
                    account = accountInfoList[0];
                    session.GetComponent<AccountsZone>().AddChild(account);
                    if (account.AccountType == (int)AccountType.BlackList)
                    {
                        session.Error = ErrorCode.ERR_LoginInfoError;
                        reply();
                        session?.DisConnect().Coroutine();
                        account.Dispose();
                        return;
                    }

                    if (!account.AccountName.Equals(request.AcccountName))
                    {
                        session.Error = ErrorCode.ERR_LoginInfoError;
                        reply();
                        session?.DisConnect().Coroutine();
                        account.Dispose();
                        return;
                    }
                    if (!account.PassWord.Equals(request.PassWord))
                    {
                        session.Error = ErrorCode.ERR_LoginInfoError;
                        reply();
                        session?.DisConnect().Coroutine();
                        account.Dispose();
                        return;
                    }
                }
                else
                {
                    account = session.GetComponent<AccountsZone>().AddChild<Account>();
                    account.AccountName = request.AcccountName.Trim();
                    account.PassWord = request.PassWord;
                    account.CreateTime = TimeHelper.ServerNow();
                    account.AccountType = (int)AccountType.Genenral;
                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                }

                StartSceneConfig sceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                long loginCenterInstanceId = sceneConfig.InstanceId;
                var loginAccountResponse =
                        (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId,
                            new A2L_LoginAccountRequest() { AccountId = account.Id });
                if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                {
                    response.Error = loginAccountResponse.Error;
                    reply();
                    session?.DisConnect().Coroutine();
                    account.Dispose();
                    return;
                }
                long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionCompontent>().Get(account.Id);
                Session outherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                outherSession?.Send(new A2C_Disconnect(){Error = 0});
                outherSession.DisConnect().Coroutine();
                session.DomainScene().GetComponent<AccountSessionCompontent>().Add(account.Id,session.InstanceId);

                session.AddComponent<AccountCheckOutTimeCompontent, long>(account.Id);
                
                string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                session.DomainScene().GetComponent<TokenComponent>().Add(account.Id,token);

                response.AccountId = account.AccountId;
                response.Token = token;
                reply();
                }
            }
        }
    }
}