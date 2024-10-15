using Managers;
using UnityEngine;
using Coffee.UIExtensions;

namespace CurrencySystem
{
    public class CurrencyRegisterer : MonoBehaviour
    {
        public int currencyAmount = 1;
    
        // Start is called before the first frame update
        void Start()
        {
            var attractors=GetComponentsInChildren<UIParticleAttractor>();
            foreach (var attractor in attractors)
            {
                attractor.onAttracted.AddListener(RegisterCurrency);
            }
        
        }

        private void RegisterCurrency()
        {
            CoreManager.Instance.EarnMoney(currencyAmount);
        }
    
    }
}
