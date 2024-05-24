using GameEvents;
using UnityEngine;

namespace Managers
{
    public class GlobalActionsBinder
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            GlobalActions.Reset();
            
            
            var gameStartEvent=FindGameEvent("OnGameStarted");
            GlobalActions.OnGameStarted += gameStartEvent.Invoke;
            
            var gameEndEvent=FindGameEvent("OnGameEnded");
            GlobalActions.OnGameEnded += gameEndEvent.Invoke;
            
            var newLevelLoadedEvent=FindGameEvent("OnNewLevelLoaded");
            GlobalActions.OnNewLevelLoaded += newLevelLoadedEvent.Invoke;
            
            var levelDestroyedEvent=FindGameEvent("OnLevelDestroyed");
            GlobalActions.OnLevelDestroyed += levelDestroyedEvent.Invoke;
            
            var gameLostEvent=FindGameEvent("OnGameLost");
            GlobalActions.OnGameLost += gameLostEvent.Invoke;
            
            var gameWinEvent=FindGameEvent("OnGameWin");
            GlobalActions.OnGameWin += gameWinEvent.Invoke;
            
            var gameRestartedEvent=FindGameEvent("OnGameRestarted");
            GlobalActions.OnGameRestarted += gameRestartedEvent.Invoke;
            
            var levelChangedEvent=FindGameEvent("OnLevelChanged");
            GlobalActions.OnLevelChanged += _ => levelChangedEvent.Invoke();
            
            var gameMoneyChangedEvent=FindGameEvent("OnGameMoneyChanged");
            GlobalActions.OnGameMoneyChanged += _ => gameMoneyChangedEvent.Invoke();
            
            var levelMoneyChangedEvent=FindGameEvent("OnLevelMoneyChanged");
            GlobalActions.OnLevelMoneyChanged += _ => levelMoneyChangedEvent.Invoke();
        }


        private static GameEvent FindGameEvent(string name)
        {
            var gameEvent = Resources.Load<GameEvent>($"GameEvents/{name}");
            return gameEvent;
        }
    }
}