using Coffee.UIExtensions;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.CurrencySystem
{
    public class CurrencyRegisterer : MonoBehaviour
    {
        public int currencyAmount = 1;
        public string earnReason = "";
    
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
            CoreManager.Instance.EarnMoney(currencyAmount,earnReason);
        }
    
    }
}
