using CorePublic.Managers;
using UnityEngine;

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

        protected void Start()
        {
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged+=OnMoneyChanged;
            }
            else if(type==Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged+=OnMoneyChanged;
                
            }else if(type==Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged += OnMoneyChanged;
                GlobalActions.OnGameMoneyChanged += OnMoneyChanged;                
            }

            UpdateMoneyText();
        }

        
        public void SetOffset(float offset){
            this.offset=offset;
            UpdateMoneyText();
        }

        public virtual void UpdateMoneyText()
        {
             if (type == Types.GameMoney)
            {
                textController.SetText(CoreManager.Instance.GameMoney+offset);
            }
            else if(type==Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                textController.SetText(LevelMoneyManager.Instance.LevelMoney+offset);
                
            }else if(type==Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                textController.SetText(LevelMoneyManager.Instance.LevelMoney+CoreManager.Instance.GameMoney+offset);
            }            
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
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged -= OnMoneyChanged;
            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged -= OnMoneyChanged;
                GlobalActions.OnGameMoneyChanged -= OnMoneyChanged;
            }
        }
    }
}