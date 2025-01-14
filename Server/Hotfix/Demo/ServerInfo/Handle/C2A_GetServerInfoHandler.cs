﻿using System;

namespace ET.Handle
{
    [FriendClass(typeof(ServerInfoManagerComponent))]
    public class C2A_GetServerInfoHandler : AMRpcHandler<C2A_GetServerInfos,A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误，当前的Scene为{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            var token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session?.DisConnect().Coroutine();
                return;
            }

            foreach (var serverInfo in session.DomainScene().GetComponent<ServerInfoManagerComponent>().ServerInfos)
            {
                response.ServerInfoList.Add(serverInfo.ToMessage());
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}