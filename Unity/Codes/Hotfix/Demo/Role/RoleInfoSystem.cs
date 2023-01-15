namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
            self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.AccountId = roleInfoProto.AccountId;
            self.State = roleInfoProto.State;
            self.CreateTime = roleInfoProto.CreateTime;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
            self.ServerId = roleInfoProto.ServerId;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto()
            {
                Id = self.Id,
                Name = self.Name,
                AccountId = self.AccountId,
                State = self.State,
                CreateTime = self.CreateTime,
                LastLoginTime = self.LastLoginTime,
                ServerId = self.ServerId,
            };
        }
    }
}