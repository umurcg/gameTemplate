using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CorePublic.Classes;
using CorePublic.Helpers;
using CorePublic.Interfaces;
using CorePublic.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


namespace CorePublic.Managers
{
    public class LevelManager : Singleton<LevelManager>, IStats
    {

        [Tooltip("If checked, level manager will load last played level automatically when game is started")]
        public bool autoLoad = true;

        /// <summary>
        /// If true, it means the level is loaded in design mode which means it was already in scene in editor and that level will be used as loaded level.
        /// </summary>
        public bool InDesignMode { get; private set; }

        protected CoreManager CoreManager;

        public int NumberOfTotalLevels { get; protected set; }

        [SerializeField] protected LevelData[] Levels;

        public LevelData ActiveLevelData { get; protected set; }

        [Tooltip("Game will be tried to start with this level ignoring game data. This is useful for fast development")]
        public int TestLevelIndex = -1;
        public int DefaultRepeatLevelIndex = -1;

        /// <summary>
        ///     Whether or not there are active loaded level in the scene
        /// </summary>
        public bool LevelIsLoaded => ActiveLevelData != null || ActiveLevelObject != null;

        public GameObject ActiveLevelObject => transform.childCount > 0 ? transform.GetChild(0).gameObject : null;


        public int LastRandomLevelIndex
        {
            get => PlayerPrefs.GetInt("LastLoadedLevelIndex", -1);
            private set => PlayerPrefs.SetInt("LastLoadedLevelIndex", value);
        }

        public bool LastRandomLevelWon
        {
            get => PlayerPrefs.GetInt("LastLoadedLevelWon", 0) == 1;
            private set => PlayerPrefs.SetInt("LastLoadedLevelWon", value ? 1 : 0);
        }

        public virtual int RepeatStartLevelIndex
        {
            get
            {
                int repeatStartLevelIndex = DefaultRepeatLevelIndex;
                if (RemoteConfig.Instance)
                    repeatStartLevelIndex =
                        RemoteConfig.Instance.GetInt("repeatStartLevelIndex", repeatStartLevelIndex);
                return repeatStartLevelIndex;
            }
        }

#if UNITY_EDITOR
        public LevelData TestLevelData;
        public bool EnableDesignMode = true;
#endif

        protected new void Awake()
        {
            base.Awake();
        }

        protected IEnumerator Start()
        {
            CoreManager = CoreManager.Request();
            NumberOfTotalLevels = Levels.Length;
            yield return null;

#if UNITY_EDITOR
            if (Application.isEditor && EnableDesignMode)
            {
                if (transform.childCount > 0)
                {
                    InDesignMode = true;
                    GlobalActions.OnNewLevelLoaded.Invoke();
                    yield break;
                }
            }

            if (TestLevelData)
            {
                LoadLevel(TestLevelData);
                GlobalActions.OnGameWin += GameWin;
                yield break;
            }
#endif

            //Error protection
            if (NumberOfTotalLevels == 0)
            {
                Debug.LogError(
                    "There is no level assigned to level manager. You have to assign level data to run the game.");
                Destroy(gameObject);
                yield break;
            }


            //If auto load is checked load level with current level automatically when game is started
            if (autoLoad)
            {
                LoadLevel(CoreManager.Level);
                GlobalActions.OnLevelChanged += LoadLevel;
                GlobalActions.OnGameRestarted += ReloadLevel;
            }

            GlobalActions.OnGameWin += GameWin;
            GlobalActions.OnGameExit += ClearLoadedLevel;

        }

        public void ReloadLevel()
        {
            LoadLevel(CoreManager.Level);
        }

        private void GameWin()
        {
            if (LastRandomLevelIndex >= 0) LastRandomLevelWon = true;
        }



        private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("new scene is laoded!");
            GlobalActions.OnNewLevelLoaded?.Invoke();
        }

        public virtual void LoadLevel(int levelIndex)
        {
            //First try to load test levels if in editor
#if UNITY_EDITOR
            if (TestLevelIndex > 0)
            {

                if (TestLevelIndex > Levels.Length)
                {
                    Debug.LogError("Test level index is out of range of levels array");
                    return;
                }

                var level = Levels[TestLevelIndex - 1];
                LoadLevel(level);
                return;
            }
#endif


            //If level index is in range of level array capacity
            if (levelIndex < Levels.Length)
            {
                LevelData level;
                level = Levels[levelIndex];
                LoadLevel(level);
            }
        }

        public int GetNextLevelIndex(int levelIndex)
        {
            if (levelIndex < NumberOfTotalLevels - 1)
            {
                return levelIndex;
            }
            else
            {
                return GetRepeatingLevelIndex();
            }
        }
        protected virtual void LoadRepeatLevel()
        {
            int repeatLevelIndex = GetRepeatingLevelIndex();
            if (IsRandomRepeating())
            {
                if (LastRandomLevelIndex < 0 || LastRandomLevelWon)
                {
                    LastRandomLevelIndex = repeatLevelIndex;
                    LastRandomLevelWon = false;
                }
            }

            LoadLevel(repeatLevelIndex);

        }

        private bool IsRandomRepeating()
        {
            int repeatStartLevelIndex = RepeatStartLevelIndex;
            return repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;
        }

        public virtual int GetRepeatingLevelIndex()
        {

            int repeatStartLevelIndex = RepeatStartLevelIndex;

            if (IsRandomRepeating())
            {
                if (LastRandomLevelIndex > -1 && !LastRandomLevelWon)
                {
                    return LastRandomLevelIndex;
                }
                else
                {
                    List<int> randomLevelIndexes = new List<int>();
                    for (int i = 0; i < NumberOfTotalLevels; i++)
                    {
                        if (LastRandomLevelIndex >= 0 && i == LastRandomLevelIndex) continue;
                        randomLevelIndexes.Add(i);
                    }

                    int randomLevelIndex = randomLevelIndexes[Random.Range(0, randomLevelIndexes.Count)];
                    return randomLevelIndex;
                }
            }
            else
            {
                var deltaLevelIndex = CoreManager.Instance.Level - NumberOfTotalLevels;
                var repeatLevelIndex =
                    repeatStartLevelIndex +
                    deltaLevelIndex % (NumberOfTotalLevels - repeatStartLevelIndex);
                return repeatLevelIndex;
            }

        }



        [ContextMenu("GetCurrentLevelOccurenceCount")]
        public int GetCurrentLevelOccurenceCount()
        {
            int repeatStartLevelIndex = RepeatStartLevelIndex;
            bool isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

            if (isRandomRepeating)
            {
                Debug.LogWarning("Current level occurence count is not valid for random repeating levels");
                return -1;
            }

            int occurenceCount = 1;
            bool isRepeating = CoreManager.Instance.Level >= NumberOfTotalLevels;
            if (isRepeating)
            {
                occurenceCount = (CoreManager.Instance.Level - NumberOfTotalLevels) /
                    (NumberOfTotalLevels - repeatStartLevelIndex) + 2;
            }

            Debug.Log("Current level occurence count: " + occurenceCount);
            return occurenceCount;
        }

#if UNITY_EDITOR
        public void LoadTestLevel()
        {
            LoadLevel(TestLevelData);
        }
#endif
        public virtual void LoadLevel(LevelData levelData)
        {
            ClearLoadedLevel();
            LoadLevelWithData(levelData);
            GlobalActions.OnNewLevelLoaded?.Invoke();
        }

        public virtual void LoadLevel(GameObject levelPrefab)
        {
            ClearLoadedLevel();
            LoadLevelPrefab(levelPrefab);
            GlobalActions.OnNewLevelLoaded?.Invoke();
        }


        /// <summary>
        ///     Clears active if level if there is loaded one
        /// </summary>
        public virtual void ClearLoadedLevel()
        {
            if (LevelIsLoaded)
            {
                //Clear all children object while all related objects will be spawned under this object
                for (var i = 0; i < transform.childCount; i++)
                    DestroyImmediate(transform.GetChild(i).gameObject);

                ActiveLevelData = null;
                GlobalActions.OnLevelDestroyed?.Invoke();
            }
        }

        public LevelData[] GetLevels()
        {
            return Levels;
        }

        public LevelData GetLevel(int index)
        {
            if (index < Levels.Length)
            {
                return Levels[index];
            }
            else
            {

                int repeatStartLevelIndex = RepeatStartLevelIndex;
                var isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

                if (isRandomRepeating) return null;

                var deltaLevelIndex = CoreManager.Instance.Level - NumberOfTotalLevels;
                var repeatLevelIndex =
                    repeatStartLevelIndex +
                    deltaLevelIndex % (NumberOfTotalLevels - repeatStartLevelIndex);
                return Levels[repeatLevelIndex];
            }
        }


        protected virtual void LoadLevelWithData(LevelData levelData)
        {
            ActiveLevelData = levelData;
            //Try to instantiate level prefab if exists
            if (levelData.levelPrefab)
            {
                LoadLevelPrefab(levelData.levelPrefab);
            }
        }

        protected virtual void LoadLevelPrefab(GameObject levelPrefab)
        {
            Instantiate(levelPrefab, transform);
        }

        private void OnDestroy()
        {
            if (autoLoad)
            {
                GlobalActions.OnLevelChanged -= LoadLevel;
            }

#if UNITY_EDITOR
            TestLevelData = null;
#endif
        }

        public string GetStats()
        {
            string stats = "";
            stats += "Is Level Loaded: " + LevelIsLoaded + "\n";
            stats += "Level Manager Design Mode: " + InDesignMode + "\n";
            if (ActiveLevelData)
            {
                stats += "Active Level: " + ActiveLevelData.levelName;
            }

            return stats;
        }
    }
}
