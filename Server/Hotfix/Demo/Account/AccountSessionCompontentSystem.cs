namespace ET
{
    [ObjectSystem]
    public class AccountSessionCompontentDestroySystem : DestroySystem<AccountSessionCompontent>
    {
        public override void Destroy(AccountSessionCompontent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }
    [FriendClass(typeof(AccountSessionCompontent))]
    public static class AccountSessionCompontentSystem
    {
        public static void Add(this AccountSessionCompontent self,long accountId, long sessionId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary[accountId] = sessionId;
                return;
            }
            self.AccountSessionDictionary.Add(accountId,sessionId);
        }

        public static long Get(this AccountSessionCompontent self, long accountId)
        {
            if (!self.AccountSessionDictionary.TryGetValue(accountId,out long instanceId))
            {
                return 0;
            }
            return instanceId;
        }

        public static void Remove(this AccountSessionCompontent self,long key)
        {
            if (self.AccountSessionDictionary.ContainsKey(key))
            {
                self.AccountSessionDictionary.Remove(key);
            }
        }
    }
}