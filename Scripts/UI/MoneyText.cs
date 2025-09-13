using CorePublic.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.UI
{
    public class MoneyText : MonoBehaviour
    {
        [SerializeField] private TextController textController;

        public enum Types
        {
            GameMoney,
            TotalMoney,
            LevelMoney,
            Custom
        }

        public Types type;
        private float offset;

        private float _lastSetMoney;
        
        public UnityEvent OnVisibleMoneyChanged;
        public UnityEvent OnVisibleMoneyIncreased;
        public UnityEvent OnVisibleMoneyDecreased;

        protected void Start()
        {
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged += OnMoneyChanged;
            }
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance != null)
            {
                GlobalActions.OnLevelMoneyChanged += OnMoneyChanged;

            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance != null)
            {
                GlobalActions.OnLevelMoneyChanged += OnMoneyChanged;
                GlobalActions.OnGameMoneyChanged += OnMoneyChanged;
            }

            UpdateMoneyText();
        }


        public void SetOffset(float offset)
        {
            this.offset = offset;
            UpdateMoneyText();
        }

        public virtual void UpdateMoneyText()
        {
            float moneyToSet = 0;
            if (type == Types.GameMoney)
            {
                moneyToSet = CoreManager.Instance.GameMoney + offset;
            }
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance != null)
            {
                moneyToSet = LevelMoneyManager.Instance.LevelMoney + offset;

            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance != null)
            {
                moneyToSet = LevelMoneyManager.Instance.LevelMoney + CoreManager.Instance.GameMoney + offset;
            }

            textController.SetText(moneyToSet);
            if (moneyToSet > _lastSetMoney)
            {
                OnVisibleMoneyIncreased?.Invoke();
            }
            else if (moneyToSet < _lastSetMoney)
            {
                OnVisibleMoneyDecreased?.Invoke();
            }

            if (moneyToSet != _lastSetMoney)
            {
                OnVisibleMoneyChanged?.Invoke();
            }

            _lastSetMoney = moneyToSet;
        }

        protected virtual void OnMoneyChanged(float amount)
        {
            UpdateMoneyText();
        }

        void OnDestroy()
        {
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged -= OnMoneyChanged;
            }
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance != null)
            {
                GlobalActions.OnLevelMoneyChanged -= OnMoneyChanged;
            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance != null)
            {
                GlobalActions.OnLevelMoneyChanged -= OnMoneyChanged;
                GlobalActions.OnGameMoneyChanged -= OnMoneyChanged;
            }
        }
    }
}