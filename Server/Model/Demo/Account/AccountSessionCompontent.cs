using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionCompontent :Entity,IAwake,IDestroy
    {
        public Dictionary<long, long> AccountSessionDictionary = new Dictionary<long, long>();
    }
}