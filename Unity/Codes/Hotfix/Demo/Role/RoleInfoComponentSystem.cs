namespace ET
{
    public class RoleInfoComponentDestroySystem: DestroySystem<RoleInfoComponent>
    {
        public override void Destroy(RoleInfoComponent self)
        {
            foreach (var tempInfo in self.RoleInfos)
            {
                tempInfo?.Dispose();
            }
            self.RoleInfos.Clear();
            self.CurrentRoleId = 0;
        }
    }

    public static class RoleInfoComponentSystem
    {
        
    }
}