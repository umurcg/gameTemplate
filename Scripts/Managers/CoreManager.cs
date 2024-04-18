using System.Collections;
using Core.Interfaces;
using Helpers;
using Lean.Touch;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    [AddComponentMenu("*Reboot/Managers/Core Manager")]
    public sealed class CoreManager : Singleton<CoreManager>, IStats
    {
        private const string LevelSaveKey = "GameLevel";
        private const string MoneySaveKey = "GameMoney";

        [SerializeField] private bool saveEnabled = true;

        [Tooltip("If checked, start state will be triggered automatically when a level loaded")] [SerializeField]
        private bool autoStart = true;

        public bool IsGameStarted { private set; get; }

        public float GameMoney
        {
            get => PlayerPrefs.GetFloat(MoneySaveKey, 0);
            private set => PlayerPrefs.SetFloat(MoneySaveKey, value);
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
            var hasLevelManager = LevelManager.Request();

            if (autoStart)
            {
                if (hasLevelManager)
                    ActionManager.Instance.OnNewLevelLoaded += StartGameNextFrame;
                else
                {
                    yield return null;
                    StartGame();
                    ActionManager.Instance.OnLevelChanged += _ => StartGameNextFrame();
                    ActionManager.Instance.OnGameRestarted += StartGameNextFrame;
                }
            }
        }


        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnNewSceneLoaded;

            if (!saveEnabled)
                PlayerPrefs.DeleteAll();

            IsFirstSession = false;
        }

        private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        public void StartGame()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Game is already started. You can not start the game after it is started already");
                return;
            }

            IsGameStarted = true;
            ActionManager.Instance.OnGameStarted?.Invoke();
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

            ActionManager.Instance.OnGameWin?.Invoke();
            ActionManager.Instance.OnGameEnded?.Invoke();
            IsGameStarted = false;
        }

        public void LostGame()
        {
            if (!IsGameStarted)
            {
                Debug.LogError("Gameplay is not started you can not trigger lost game action");
                return;
            }

            ActionManager.Instance.OnGameLost?.Invoke();
            ActionManager.Instance.OnGameEnded?.Invoke();
            IsGameStarted = false;
        }

        public void EarnMoney(float amount)
        {
            GameMoney += amount;
            ActionManager.Instance.OnGameMoneyChanged?.Invoke(GameMoney);
        }

        public bool SpendMoney(float amount)
        {
            if (amount <= GameMoney)
            {
                GameMoney -= amount;
                ActionManager.Instance.OnGameMoneyChanged?.Invoke(GameMoney);
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
            ActionManager.Instance.OnLevelChanged?.Invoke(Level);
        }

        public void SetLevel(int levelIndex, bool preventCallback = false)
        {
            Level = levelIndex;
            if (!preventCallback)
                ActionManager.Instance.OnLevelChanged?.Invoke(Level);
        }

        public void ReloadLevel()
        {
            if (IsGameStarted)
            {
                Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
                return;
            }

            ActionManager.Instance.OnLevelChanged?.Invoke(Level);
            ActionManager.Instance.OnGameRestarted?.Invoke();
        }

        public string GetStats()
        {
            var versionStats = "v " + Application.version;
            var gamePlayStats = "Game Started: " + IsGameStarted;
            return versionStats + "\n" + gamePlayStats;
        }
    }
}