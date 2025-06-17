using CorePublic.Helpers;

namespace CorePublic.Managers
{
    public class LevelMoneyManager : Singleton<LevelMoneyManager>
    {
        private float _levelMoney;

        public float LevelMoney
        {
            get => _levelMoney;
            set
            {
                _levelMoney = value;
                GlobalActions.OnLevelMoneyChanged?.Invoke(_levelMoney);
            }
        }
    }
}