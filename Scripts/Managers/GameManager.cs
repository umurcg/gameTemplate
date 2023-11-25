using System;
using Helpers;
using Managers;
using UnityEngine;

namespace Core.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private float _levelMoney;

        public float LevelMoney
        {
            get => _levelMoney;
            set
            {
                _levelMoney = value;
                ActionManager.Instance.OnLevelMoneyChanged?.Invoke(_levelMoney);
            }
        }


        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.L))
                LevelMoney+=1000;
#endif
        }

        public void IncreaseTimeScale()
        {
            Time.timeScale += 1;
            Time.fixedDeltaTime = .02f * Time.timeScale;
            Debug.Log("Time scale increased to " + Time.timeScale);
        }
        
        
        public void DecreaseTimeScale()
        {
            Time.timeScale -= 1;
            Time.fixedDeltaTime = .02f * Time.timeScale;
            Debug.Log("Time scale decreased to " + Time.timeScale);
        }
    }
}