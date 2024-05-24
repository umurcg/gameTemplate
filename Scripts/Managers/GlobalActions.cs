using System;
using Core.Interfaces;
using Helpers;

namespace Managers
{
    public class GlobalActions
    {
        public static Action OnGameStarted;
        public static Action OnGameEnded;
        public static Action OnNewLevelLoaded;
        public static Action OnLevelDestroyed;
        public static Action OnGameLost;
        public static Action OnGameWin;
        public static Action OnGameRestarted;
        public static Action<int> OnLevelChanged;
        public static Action<float> OnGameMoneyChanged;
        public static Action<float> OnLevelMoneyChanged;
        
        
        public static void Reset()
        {
            OnGameStarted = null;
            OnGameEnded = null;
            OnNewLevelLoaded = null;
            OnLevelDestroyed = null;
            OnGameLost = null;
            OnGameWin = null;
            OnGameRestarted = null;
            OnLevelChanged = null;
            OnGameMoneyChanged = null;
            OnLevelMoneyChanged = null;
        }
        
        
        
    }
}