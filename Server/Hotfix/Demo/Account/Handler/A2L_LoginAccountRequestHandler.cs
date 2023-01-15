using System;

namespace ET
{
    [ActorMessageHandler]
    public class A2L_LoginAccountRequestHandler : AMActorRpcHandler<Scene,A2L_LoginAccountRequest,L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene unit, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock,accountId.GetHashCode()))
            {
                if (!unit.GetComponent<LoginInfoRecordCompontent>().IsExist(accountId))
                {
                    reply();
                    return;
                }

                int zone = unit.GetComponent<LoginInfoRecordCompontent>().Get(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone,accountId);
                
                var g2LDisconnectGate =
                        (G2L_DisconnectGateUnit)await MessageHelper.CallActor(gateConfig.InstanceId,
                            new L2G_DisconnectGateUnit() { AccountId = accountId });
                response.Error = g2LDisconnectGate.Error;
                reply();
            }
            await ETTask.CompletedTask;
        }
    }
}