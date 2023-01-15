namespace ET
{
    [ObjectSystem]
    public class LoginInfoRecordCompontentDestroySystem : DestroySystem<LoginInfoRecordCompontent>
    {
        public override void Destroy(LoginInfoRecordCompontent self)
        {
            self.AccountLoginInfoDict.Clear();
        }
    }
    [FriendClass(typeof(LoginInfoRecordCompontent))]
    public static class LoginInfoRecordCompontentSystem
    {
        public static void Add(this LoginInfoRecordCompontent self, long key, int value)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict[key] = value;
            }
            else
            {
                self.AccountLoginInfoDict.Add(key,value);
            }
        }
        public static void Remove(this LoginInfoRecordCompontent self, long key)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict.Remove(key);
            }
        }
        
        public static int Get(this LoginInfoRecordCompontent self, long key)
        {
            if (!self.AccountLoginInfoDict.TryGetValue(key,out int value))
            {
                return -1;
            }

            return value;
        }
        
        public static bool IsExist(this LoginInfoRecordCompontent self, long key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }
    }
}