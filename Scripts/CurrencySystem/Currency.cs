using System;
using UnityEngine;

namespace CurrencySystem
{
    [Serializable]
    public class Currency
    {
        public string name;
        public float defaultValue;
        public float Value => PlayerPrefs.GetFloat(name, defaultValue);
        public Action<float> OnValueChanged { get; set; }
        public Action<float> OnNonRegisteredValueChanged { get; set; }
        public Action OnValueRegistered { get; set; }
        public Sprite icon;

        public float NonRegisteredValue { get; private set; }

        public bool Spend(float amount)
        {
            if (!CanSpend(amount))
            {
                return false;
            }

            var newValue = Value - amount;
            newValue = Mathf.Max(0, newValue);
            PlayerPrefs.SetFloat(name, newValue);
            OnValueChanged?.Invoke(newValue);
            return true;
        }

        public void Earn(float amount, bool register = true)
        {
            if (register)
            {
                var newValue = Value + amount;
                PlayerPrefs.SetFloat(name, newValue);
                OnValueChanged?.Invoke(newValue);
            }
            else
            {
                var newValue = NonRegisteredValue + amount;
                NonRegisteredValue = newValue;
                OnNonRegisteredValueChanged?.Invoke(newValue);
            }
        }
        
        public void RegisterNonRegisteredValue()
        {
            var newValue = Value + NonRegisteredValue;
            PlayerPrefs.SetFloat(name, newValue);
            OnValueChanged?.Invoke(newValue);
            NonRegisteredValue = 0;
            OnValueRegistered?.Invoke();
            OnNonRegisteredValueChanged?.Invoke(0);
        }

        public bool CanSpend(float amount)
        {
            return Value >= amount;
        }

        public void SetNonRegisteredValue(float value)
        {
            NonRegisteredValue = value;
            OnNonRegisteredValueChanged?.Invoke(value);
        }
    }
}