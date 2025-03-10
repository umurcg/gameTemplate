using System.Collections;
using CorePublic.CurrencySystem;
using CorePublic.Helpers;
using CorePublic.Interfaces;
using UnityEngine;

namespace CorePublic.Managers
{
    [AddComponentMenu("*Reboot/Managers/Core Manager")]
    public sealed class CoreManager : Singleton<CoreManager>, IStats
    {
        
        private static string LevelSaveKey = "GameLevel";
        private static string LastSavedGameStateKey = "LastSavedGameState";
        private static string IsFirstSessionKey = "IsFirstSession";

        [SerializeField] private bool saveEnabled = true;

        [Tooltip("If checked, start state will be triggered automatically when a level loaded")] [SerializeField]
        private bool autoStart = true;

        public static GameStates LastSavedGameState{
            get => (GameStates)PlayerPrefs.GetInt(LastSavedGameStateKey, (int)GameStates.Idle);
            set => PlayerPrefs.SetInt(LastSavedGameStateKey, (int)value);
        }        

        public GameStates GameState { get; private set; } = GameStates.Idle;

        public bool IsGameStarted => GameState == GameStates.InGame;
        
        private bool _hasLevelManager;

        public float GameMoney
        {
            get
            {
                var currency = GetMainCurrency();
                if (currency == null)
                {
                    Debug.LogError("Currency is not found in the currency data");
                    return 0;
                }

                return currency.Value;
            }
            private set
            {
                var currency = GetMainCurrency();
                if (currency == null)
                {
                    Debug.LogError("Currency is not found in the currency data");
                    return;
                }

                currency.Earn(value - currency.Value);
            }
        }

        
        public int Level => GameLevel;
        
        public static int GameLevel
        {
            get => PlayerPrefs.GetInt(LevelSaveKey, 0);
            private set => PlayerPrefs.SetInt(LevelSaveKey, value);
        }

        public static bool IsFirstSession
        {
            get => PlayerPrefsX.GetBool(IsFirstSessionKey, true);
            private set => PlayerPrefsX.SetBool(IsFirstSessionKey, value);
        }

        /// <summary>
        /// The number of times the game has been lost in current level.
        /// </summary>
        public int LostCounter { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Screen.orientation = ScreenOrientation.Portrait;
            QualitySettings.vSyncCount = 0;
        }

        private IEnumerator Start()
        {
            _hasLevelManager = LevelManager.Request();

            if (autoStart)
            {
                if (_hasLevelManager)
                    GlobalActions.OnNewLevelLoaded += StartGameNextFrame;
                else
                {
                    yield return null;
                    GlobalActions.OnLevelChanged += StartGameNextFrame;
                    GlobalActions.OnGameRestarted += StartGameNextFrame;
                    StartGameNextFrame();
                }
            }
        }


        public void OnDestroy()
        {
            if (!saveEnabled)
                PlayerPrefs.DeleteAll();

            if (autoStart)
            {
                if (_hasLevelManager)
                    GlobalActions.OnNewLevelLoaded -= StartGameNextFrame;
                else
                {
                    GlobalActions.OnLevelChanged -= StartGameNextFrame;
                    GlobalActions.OnGameRestarted -= StartGameNextFrame;
                }
            }

            IsFirstSession = false;
        }


        

        public void StartGame()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Game is already started. You can not start the game after it is started already");
                return;
            }

            LostCounter = 0;
            GameState = GameStates.InGame;
            LastSavedGameState = GameState;
            GlobalActions.OnGameStarted?.Invoke();
        }
        
        private void StartGameNextFrame(int arg)
        {
            StartCoroutine(_StartGameNextFrame());
        }

        private void StartGameNextFrame()
        {
            StartCoroutine(_StartGameNextFrame());
        }

        private IEnumerator _StartGameNextFrame()
        {
            yield return null;
            StartGame();
        }

        public void WinGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not trigger win game action");
                return;
            }

            GlobalActions.OnGameWin?.Invoke();
            GlobalActions.OnGameEnded?.Invoke();
            GameState = GameStates.Win;
            LastSavedGameState = GameState;
        }

        public void LostGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not trigger lost game action");
                return;
            }

            LostCounter++;
            GameState = GameStates.Lost;
            LastSavedGameState = GameState;
            GlobalActions.OnGameLost?.Invoke();
            GlobalActions.OnGameEnded?.Invoke();
        }

        public void ConfirmLost()
        {
            if (GameState != GameStates.Lost)
            {
                Debug.LogError("Game is not lost you can not confirm lost");
                return;
            }
            
            GameState = GameStates.DefiniteLost;
            LastSavedGameState = GameState;
            GlobalActions.OnGameLostConfirmed?.Invoke();
        }

        public void ExitGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not exit the game");
                return;
            }
            
            SetStateToIdle();

            GlobalActions.OnGameExit?.Invoke();
            GlobalActions.OnGameEnded?.Invoke();
        }
        
        public void PauseGame()
        {
            if(GameState != GameStates.InGame)
            {
                Debug.LogError("Game is not in game you can not pause the game");
                return;
            }

            GameState = GameStates.Pause;
            LastSavedGameState = GameState;
            GlobalActions.OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            if(GameState != GameStates.Pause)
            {
                Debug.LogError("Game is not paused you can not resume the game");
                return;
            }

            GameState = GameStates.InGame;
            LastSavedGameState = GameState;
            GlobalActions.OnGameResumed?.Invoke();
        }

        public void EarnMoney(float amount, string reason="")
        {
            GameMoney += amount;
            GlobalActions.OnGameMoneyChanged?.Invoke(GameMoney);
            GlobalActions.OnGameCurrencyEarn?.Invoke(reason, amount);
        }
        

        public void SetStateToIdle()
        {
            GameState = GameStates.Idle;
            LastSavedGameState = GameState;
        }

        public void SpendMoney(float amount, string reason="")
        {
            if (amount > GameMoney)
            {
                Debug.LogError("Not enough money to spend");
                return;
            }

            GameMoney -= amount;
            GlobalActions.OnGameMoneyChanged?.Invoke(GameMoney);
            GlobalActions.OnGameCurrencySpend?.Invoke(reason, amount);
        }

        
        public static void ClearSaveData()
        {
            Debug.Log("Save data cleared!");
            PlayerPrefs.DeleteAll();
        }

        public static void IncreaseLevel()
        {
            if (CoreManager.Instance && CoreManager.Instance.IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
                return;
            }
            
            GlobalActions.OnPreLevelIncrease?.Invoke();
            GameLevel++;
            GlobalActions.OnLevelChanged?.Invoke(GameLevel);
        }

        public static void SetLevel(int levelIndex, bool preventCallback = false)
        {
            GameLevel = levelIndex;
            if (!preventCallback)
                GlobalActions.OnLevelChanged?.Invoke(GameLevel);
        }

        public void RestartGame()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
                return;
            }
        
            GlobalActions.OnGameRestarted?.Invoke();
        }

        public void Revive()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not revive during a gameplay");
                return;
            }

            if (GameState!=GameStates.Lost)
            {
                Debug.LogError("Game state is not Lost. You can not revive the game");
                return;
            }
            
            GameState = GameStates.InGame;
            LastSavedGameState = GameState;
            GlobalActions.OnGameRevived?.Invoke();
        }
        
        public static Currency GetMainCurrency()
        {
            var currencyData = CurrencyData.Instance;
            if(currencyData == null) return null;
            if (currencyData.currencies==null || currencyData.currencies.Length == 0)
            {
                Debug.LogWarning("There is no currency in the currency data, Creating a currency in the currency data with name 'MainMoney'");
                currencyData.currencies = new Currency[1];
                currencyData.currencies[0] = new Currency();
                currencyData.currencies[0].name = "MainMoney";
            }
            
            return currencyData.currencies[0];
        }


        public string GetStats()
        {
            var versionStats = "v " + Application.version;
            var gamePlayStats = "Game State: " + GameState;
            var level = "Level: " + Level;
            var money = "Money: " + GameMoney;
            return versionStats + "\n" + gamePlayStats + "\n" + level + "\n" + money;
        }
    }
}