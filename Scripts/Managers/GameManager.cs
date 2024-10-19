using CorePublic.Helpers;

namespace CorePublic.Managers
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
                GlobalActions.OnLevelMoneyChanged?.Invoke(_levelMoney);
            }
        }
    }
}