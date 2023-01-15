﻿using System;
using MongoDB.Driver.Core.Servers;

namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem: AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }
    
    public class ServerInfoManagerComponentDestroySystem: DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var serverInfo in self.ServerInfos)
            {
                serverInfo?.Dispose();
            }
            self.ServerInfos.Clear();
        }
    }

    public class ServerInfoManagerComponentLoadSystem: LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    [FriendClass(typeof(ServerInfo))]
    [FriendClass(typeof(ServerInfoManagerComponent))]
    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            var serverInfoList = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if (serverInfoList == null || serverInfoList.Count <= 0)
            {
                Log.Error("serverInfo null");

                self.ServerInfos.Clear();
                var serverInfoConfig = ServerInfoConfigCategory.Instance.GetAll();
                foreach (var tempConfig in serverInfoConfig.Values)
                {
                    ServerInfo serverInfo = self.AddChildWithId<ServerInfo>(tempConfig.Id);
                    serverInfo.ServerName = tempConfig.ServerName;
                    serverInfo.Status = (int)ServerStatus.Normal;
                    self.ServerInfos.Add(serverInfo);
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(serverInfo);
                }
                return;
            }
            self.ServerInfos.Clear();
            foreach (var serverInfo in serverInfoList)
            {
                self.AddChild(serverInfo);
                self.ServerInfos.Add(serverInfo);
            }
            
            await ETTask.CompletedTask;
        }
    }
}