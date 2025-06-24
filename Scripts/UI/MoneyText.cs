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
        private float offset;

        protected void Start()
        {
            _coreManager = CoreManager.Request();
            _mainCamera = Camera.main;
            
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
                SetText(_coreManager.GameMoney+offset);
            }
            else if(type==Types.LevelMoney && LevelMoneyManager.Instance!=null)
            {
                SetText(LevelMoneyManager.Instance.LevelMoney+offset);
                
            }else if(type==Types.TotalMoney && LevelMoneyManager.Instance!=null)
            {
                SetText(LevelMoneyManager.Instance.LevelMoney+_coreManager.GameMoney+offset);
            }            
        }


        protected virtual void OnMoneyChanged(float amount)
        {
            UpdateMoneyText();
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