using System;

namespace CorePublic.Managers
{
    public static class GlobalActions
    {
        public static Action OnGameStarted;
        public static Action OnGameEnded;
        public static Action OnNewLevelLoaded;
        public static Action OnLevelDestroyed;
        public static Action OnGameLost;
        public static Action OnGameLostConfirmed;
        public static Action OnGameWin;
        public static Action OnGameExit;
        public static Action OnGameRestarted;
        public static Action OnGameRevived;
        public static Action OnPreLevelIncrease;
        public static Action<int> OnLevelChanged;
        public static Action<float> OnGameMoneyChanged;
        public static Action<float> OnLevelMoneyChanged;
        public static Action<string, float> OnGameCurrencySpend;
        public static Action<string, float> OnGameCurrencyEarn;

        public static Action OnGamePaused;
        public static Action OnGameResumed;

        public static void Reset()
        {
            OnGameStarted = null;
            OnGameEnded = null;
            OnNewLevelLoaded = null;
            OnLevelDestroyed = null;
            OnGameLost = null;
            OnGameWin = null;
            OnGameRestarted = null;
            OnGameRevived = null;
            OnGameExit = null;
            OnLevelChanged = null;
            OnGameMoneyChanged = null;
            OnLevelMoneyChanged = null;
            OnGameCurrencySpend = null;
            OnGameCurrencyEarn = null;
            OnGamePaused = null;
            OnGameResumed = null;
        }
    }
}