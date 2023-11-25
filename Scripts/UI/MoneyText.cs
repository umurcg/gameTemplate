using Core.Managers;
using Core.UI;
using Managers;
using UnityEngine;

namespace UI
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
        private GameManager _gameManager;

        private Camera _mainCamera;


        
        private void OnEnable()
        {
            UIManager.Instance.activeMoneyText = this;
        }

        private void Start()
        {
            _coreManager = CoreManager.Request();
            _gameManager = GameManager.Request();

            _mainCamera = Camera.main;
            
            if (type == Types.GameMoney)
            {
                ActionManager.Instance.OnGameMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(_coreManager.GameMoney);
            }
            else if(type==Types.LevelMoney)
            {
                ActionManager.Instance.OnLevelMoneyChanged+=MoneyIsChanged;
                MoneyIsChanged(_gameManager.LevelMoney);
                
            }else if(type==Types.TotalMoney)
            {
                ActionManager.Instance.OnLevelMoneyChanged += MoneyIsChanged;
                ActionManager.Instance.OnGameMoneyChanged += MoneyIsChanged;
                MoneyIsChanged(_gameManager.LevelMoney+_coreManager.GameMoney);
            }
        }
        

        private void MoneyIsChanged(float amount)
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
    }
}