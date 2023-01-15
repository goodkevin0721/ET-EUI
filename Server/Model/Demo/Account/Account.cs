namespace ET
{
    public enum AccountType
    {
        Genenral =1,
        BlackList =2,
    }

    public class Account : Entity, IAwake
    {
        public string AccountName;

        public string PassWord;

        public long CreateTime;

        public int AccountId;

        public int AccountType;
    }
}