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
        
        private const string LevelSaveKey = "GameLevel";

        [SerializeField] private bool saveEnabled = true;

        [Tooltip("If checked, start state will be triggered automatically when a level loaded")] [SerializeField]
        private bool autoStart = true;
        
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

   
        public int Level
        {
            get => PlayerPrefs.GetInt(LevelSaveKey, 0);
            private set => PlayerPrefs.SetInt(LevelSaveKey, value);
        }

        public bool IsFirstSession
        {
            get => PlayerPrefsX.GetBool("IsFirstSession", true);
            private set => PlayerPrefsX.SetBool("IsFirstSession", value);
        }

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

            GameState = GameStates.InGame;
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
        }

        public void LostGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not trigger lost game action");
                return;
            }

            
            GameState = GameStates.Lost;
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
            GlobalActions.OnGameLostConfirmed?.Invoke();
        }

        public void ExitGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not exit the game");
                return;
            }
            
            GameState = GameStates.Idle;
            
            GlobalActions.OnGameExit?.Invoke();
            GlobalActions.OnGameEnded?.Invoke();
        }
        
        public void EarnMoney(float amount)
        {
            GameMoney += amount;
            GlobalActions.OnGameMoneyChanged?.Invoke(GameMoney);
        }

        public bool SpendMoney(float amount)
        {
            if (amount <= GameMoney)
            {
                GameMoney -= amount;
                GlobalActions.OnGameMoneyChanged?.Invoke(GameMoney);
                return true;
            }

            return false;
        }

        public void ClearSaveData()
        {
            Debug.Log("Save data cleared!");
            PlayerPrefs.DeleteAll();
        }

        public void IncreaseLevel()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
                return;
            }

            Level++;
            GlobalActions.OnLevelChanged?.Invoke(Level);
        }

        public void SetLevel(int levelIndex, bool preventCallback = false)
        {
            Level = levelIndex;
            if (!preventCallback)
                GlobalActions.OnLevelChanged?.Invoke(Level);
        }

        public void ReloadLevel()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
                return;
            }

            GlobalActions.OnLevelChanged?.Invoke(Level);
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
            GlobalActions.OnGameRevived?.Invoke();
        }
        
        public Currency GetMainCurrency()
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