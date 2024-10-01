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
            foreach (var currency in currencies)
            {
                names[currencies.Length] = currency.name;
            }

            return names;
        }
    }
}