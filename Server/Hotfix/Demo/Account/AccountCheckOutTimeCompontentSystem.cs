using System;

namespace ET
{
    [Timer(TimerType.AccountSessionCheckOutTimer)]
    public class AccountSessionCheckOutTimer: ATimer<AccountCheckOutTimeCompontent>
    {
        public override void Run(AccountCheckOutTimeCompontent self)
        {
            try
            {
                self.DeleteSessoion();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    [ObjectSystem]
    public class AccountCheckOutTimeCompontentAwakeSystem :AwakeSystem<AccountCheckOutTimeCompontent,long>
    {
        public override void Awake(AccountCheckOutTimeCompontent self, long accountId)
        {
            self.AccountId = accountId;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckOutTimer,self);
        }
    }
    [ObjectSystem]
    public class AccountCheckOutTimeCompontentDestroySystem :DestroySystem<AccountCheckOutTimeCompontent>
    {
        public override void Destroy(AccountCheckOutTimeCompontent self)
        {
            self.Timer = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }
    [FriendClass(typeof(AccountCheckOutTimeCompontent))]
    public static class AccountCheckOutTimeCompontentSystem
    {
        public static void DeleteSessoion(this AccountCheckOutTimeCompontent self)
        {
            Session session = self.GetParent<Session>();
            long sessionInstanceId = session.DomainScene().GetComponent<AccountSessionCompontent>().Get(self.Id);
            if (session.InstanceId == sessionInstanceId)
            {
                session.DomainScene().GetComponent<AccountSessionCompontent>().Remove(self.Id);
            }
            session.Send(new A2C_Disconnect(){Error = 1});
            session.DisConnect().Coroutine();
        }
    }
}