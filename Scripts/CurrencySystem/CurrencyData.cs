using ScriptableObjects;
using UnityEngine;

namespace CurrencySystem
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Reboot/CurrencyData")]
    public class CurrencyData: ResourceBase<CurrencyData>
    {
        protected override string RemoteConfigKey { get; }
        public Currency[] currencies;
        
        public Currency GetCurrency(string currencyName)
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                if (currencies[i].name == currencyName)
                {
                    return currencies[i];
                }
            }

            return null;
        }
        
        
        public string[] GetCurrencyNames()
        {
            var names = new string[currencies.Length];
            for (int index = 0; index < currencies.Length; index++)
            {
                Currency currency = currencies[index];
                names[index] = currency.name;
            }

            return names;
        }
    }
}