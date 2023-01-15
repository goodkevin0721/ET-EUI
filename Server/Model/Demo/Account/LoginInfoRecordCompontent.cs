using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class LoginInfoRecordCompontent: Entity,IAwake,IDestroy
    {
        public Dictionary<long, int> AccountLoginInfoDict = new Dictionary<long, int>();
    }
}