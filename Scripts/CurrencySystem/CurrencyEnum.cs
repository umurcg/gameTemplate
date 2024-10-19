using System;

namespace CorePublic.CurrencySystem
{
    [Serializable]
    public class CurrencyEnum
    {
        public string value = "NoCurrency";
        public static CurrencyEnum GetEnum(string type)
        {
            return new CurrencyEnum {value = type};
        }
    }
}