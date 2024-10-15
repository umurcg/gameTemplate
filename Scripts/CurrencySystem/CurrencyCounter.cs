using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CurrencySystem
{
    public class CurrencyCounter : MonoBehaviour
    {
        [SerializeField] private TextController textController;
        [SerializeField] private Image currencyIcon;
        
        public CurrencyEnum currency;
        private Currency _currency;
        
        public enum CounterTypes
        {
            Registered,
            Unregistered,
            Total
        }
        
        public CounterTypes counterType=CounterTypes.Registered;
        
        protected void OnEnable()
        {
            _currency = CurrencyData.Instance.GetCurrency(currency.value);
            if (currencyIcon) currencyIcon.sprite = _currency.icon;
            
            if (counterType==CounterTypes.Registered)
            {
                _currency.OnValueChanged += OnValueChanged;
            }
            else if (counterType==CounterTypes.Unregistered)
            {
                _currency.OnNonRegisteredValueChanged += OnValueChanged;
            }
            else
            {
                _currency.OnValueChanged += OnValueChanged;
                _currency.OnNonRegisteredValueChanged += OnValueChanged;
            }

            
            UpdateCurrencyText();
        }

        protected void UpdateCurrencyText()
        {
            float value = 0;
            switch (counterType)
            {
                case CounterTypes.Registered:
                    value = _currency.Value;
                    break;
                case CounterTypes.Unregistered:
                    value = _currency.NonRegisteredValue;
                    break;
                case CounterTypes.Total:
                    value = _currency.Value + _currency.NonRegisteredValue;
                    break;
            }
            
            textController.SetText(value);
        }

        protected void OnDisable()
        {
            if (_currency == null) return;
            
            if (counterType==CounterTypes.Registered)
            {
                _currency.OnValueChanged -= OnValueChanged;
            }
            else if (counterType==CounterTypes.Unregistered)
            {
                _currency.OnNonRegisteredValueChanged -= OnValueChanged;
            }
            else
            {
                _currency.OnValueChanged -= OnValueChanged;
                _currency.OnNonRegisteredValueChanged -= OnValueChanged;
            }
        }

        private void OnValueChanged(float value)
        {
            textController.SetText(value);
        }
        

        private void OnValidate()
        {
            if (currencyIcon && currencyIcon.sprite == null)
            {
                var currencyData = CurrencyData.Instance.GetCurrency(this.currency.value);
                if (currencyData != null && currencyData.icon)
                {
                    currencyIcon.sprite = currencyData.icon;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }
    }
}