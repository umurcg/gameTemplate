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
    }
}