using System;

namespace ET
{
    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        public static void Add(this TokenComponent self,long key, string token)
        {
            self.TokenDictionary.Add(key,token);
            self.TimeOutRemoveToken(key, token).Coroutine();
        }

        public static string Get(this TokenComponent self, long key)
        {
            string token = String.Empty;
            self.TokenDictionary.TryGetValue(key, out token);
            return token;
        }

        public static void Remove(this TokenComponent self,long key)
        {
            if (self.TokenDictionary.ContainsKey(key))
            {
                self.TokenDictionary.Remove(key);
            }
        }

        private static async ETTask TimeOutRemoveToken(this TokenComponent self,long key, string token)
        {
            await TimerComponent.Instance.WaitAsync(600000);
            string onlineToken = self.Get(key);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == token)
            {
                self.Remove(key);
            }
        }

    }
}

