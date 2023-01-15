namespace ET
{
    public class ServerInfosCompontentDestroySystem : DestroySystem<ServerInfosCompontent>
    {
        public override void Destroy(ServerInfosCompontent self)
        {
            foreach (var serverInfo in self.ServerInfoList)
            {
                serverInfo?.Dispose();
            }
            self.ServerInfoList.Clear();
        }
    }
    
    [FriendClass(typeof(ServerInfo))]
    [FriendClass(typeof(ServerInfosCompontent))]
    public static class ServerInfosCompontentSystem
    {
        public static void Add(ServerInfosCompontent self, ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }
    }
}