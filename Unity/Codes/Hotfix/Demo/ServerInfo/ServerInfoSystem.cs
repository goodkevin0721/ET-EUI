namespace ET
{
    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self,ServerInfoProto serverInfoProto)
        {
            self.Id = serverInfoProto.id;
            self.Status = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            return new ServerInfoProto(){id = (int)self.Id,Status = self.Status,ServerName = self.ServerName};
        }
    }
}