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
        private LevelMoneyManager _gameManager;

        private Camera _mainCamera;
        
        protected void Start()
        {
            _coreManager = CoreManager.Request();
            _gameManager = LevelMoneyManager.Request();

            _mainCamera = Camera.main;
            
            if (type == Types.GameMoney)
            {
                GlobalActions.OnGameMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(_coreManager.GameMoney);
            }
            else if(type==Types.LevelMoney)
            {
                GlobalActions.OnLevelMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(_gameManager.LevelMoney);
                
            }else if(type==Types.TotalMoney)
            {
                GlobalActions.OnLevelMoneyChanged += MoneyIsChanged;
                GlobalActions.OnGameMoneyChanged += MoneyIsChanged;
                MoneyIsChanged(_gameManager.LevelMoney+_coreManager.GameMoney);
            }
        }
        

        protected virtual void MoneyIsChanged(float amount)
        {
            if (type == Types.GameMoney)
            {
                SetText(_coreManager.GameMoney);
            }
            else if (type == Types.LevelMoney)
            {
                SetText(_gameManager.LevelMoney);
            }
            else if (type == Types.TotalMoney)
            {
                SetText((_gameManager.LevelMoney + _coreManager.GameMoney));
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
            else if (type == Types.LevelMoney)
            {
                GlobalActions.OnLevelMoneyChanged -= MoneyIsChanged;
            }
            else if (type == Types.TotalMoney)
            {
                GlobalActions.OnLevelMoneyChanged -= MoneyIsChanged;
                GlobalActions.OnGameMoneyChanged -= MoneyIsChanged;
            }
        }
    }
}