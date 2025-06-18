using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.UI
{
    public class MoneyText : TextController
    {
        public enum Types
        {
            GameMoney,
            TotalMoney,
            LevelMoney,
            Custom
        }

        public Types type;
        private CoreManager _coreManager;

        private Camera _mainCamera;
        
        protected void Start()
        {
            _coreManager = CoreManager.Request();

            _mainCamera = Camera.main;
            
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(_coreManager.GameMoney);
            }
            else if(type==Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(LevelMoneyManager.Instance.LevelMoney);
                
            }else if(type==Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged += MoneyIsChanged;
                GlobalActions.OnGameMoneyChanged += MoneyIsChanged;
                MoneyIsChanged(LevelMoneyManager.Instance.LevelMoney+_coreManager.GameMoney);
            }
        }
        

        protected virtual void MoneyIsChanged(float amount)
        {
            if (type == Types.GameMoney)
            {
                SetText(_coreManager.GameMoney);
            }
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                SetText(LevelMoneyManager.Instance.LevelMoney);
            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                SetText((LevelMoneyManager.Instance.LevelMoney + _coreManager.GameMoney));
            }
        }

  

        public Vector3 GetWorldPoint()
        {
            Vector3 pos2D=new Vector3(transform.position.x,transform.position.y,20);
            return _mainCamera.ScreenToWorldPoint(pos2D);   
        }

        void OnDestroy()
        {
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged -= MoneyIsChanged;
            }
            else if (type == Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged -= MoneyIsChanged;
            }
            else if (type == Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                GlobalActions.OnLevelMoneyChanged -= MoneyIsChanged;
                GlobalActions.OnGameMoneyChanged -= MoneyIsChanged;
            }
        }
    }
}