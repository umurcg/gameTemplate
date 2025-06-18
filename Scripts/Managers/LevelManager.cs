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
    public class LevelManager : BaseManager<LevelManager>, IStats
    {

        [Tooltip("If checked, level manager will load last played level automatically when game is started")]
        public bool autoLoad = true;

        /// <summary>
        /// If true, it means the level is loaded in design mode which means it was already in scene in editor and that level will be used as loaded level.
        /// </summary>
        public bool InDesignMode { get; private set; }

        protected CoreManager CoreManager;

        public int NumberOfTotalLevels => LevelFunnelLibrary.GetNumberOfTotalLevelsOfCurrentFunnel();

        [SerializeField] protected LevelFunnelLibrary LevelFunnelLibrary;

        public LevelData ActiveLevelData { get; protected set; }

        /// <summary>
        ///  Whether or not there are active loaded level in the scene
        /// </summary>
        public bool LevelIsLoaded => ActiveLevelData != null || ActiveLevelObject != null;

        public GameObject ActiveLevelObject => transform.childCount > 0 ? transform.GetChild(0).gameObject : null;


#if UNITY_EDITOR
        public LevelData TestLevelData;
        [Tooltip("If checked, level manager will load level in design mode which means it will not load from Level Data, but it will behave as if there is a level loaded.")]
        public bool EnableDesignMode = true;
#endif

        protected IEnumerator Start()
        {
            CoreManager = CoreManager.Request();
            yield return null;

#if UNITY_EDITOR
            if (Application.isEditor && EnableDesignMode)
            {
                InDesignMode = true;
                GlobalActions.OnNewLevelLoaded.Invoke();
                yield break;
            }

            if (TestLevelData)
            {
                LoadLevel(TestLevelData);
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
                LoadCurrentLevel();
                GlobalActions.OnLevelChanged += OnLevelChanged;
                GlobalActions.OnGameRestarted += ReloadLevel;
            }

            GlobalActions.OnGameExit += ClearLoadedLevel;

        }

        private void OnLevelChanged(int levelIndex)
        {
            LoadCurrentLevel();
        }

        public void ReloadLevel()
        {
            LoadCurrentLevel();
        }


        private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("new scene is laoded!");
            GlobalActions.OnNewLevelLoaded?.Invoke();
        }

        public virtual void LoadCurrentLevel()
        {
            int levelIndex = GetCurrentLevelIndex();

            //If level index is in range of level array capacity
            if (levelIndex < NumberOfTotalLevels)
            {
                LevelData level;
                level = LevelFunnelLibrary.GetLevel(levelIndex);
                LoadLevel(level);
            }
            else
            {
                Debug.LogError("Level index is out of range");
            }
        }

        private int GetCurrentLevelIndex()
        {
            int levelIndex = CoreManager.Level;
            if (levelIndex < NumberOfTotalLevels)
            {
                return levelIndex;
            }
            else
            {
                return GetRepeatingLevelIndex();
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


        public virtual int GetRepeatingLevelIndex()
        {

            int repeatStartLevelIndex = LevelFunnelLibrary.GetCurrentLevelFunnel().DefaultRepeatStartLevelIndex;
            var deltaLevelIndex = CoreManager.Instance.Level - NumberOfTotalLevels;
            var repeatLevelIndex =
                repeatStartLevelIndex +
                deltaLevelIndex % (NumberOfTotalLevels - repeatStartLevelIndex);
            return repeatLevelIndex;
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

        public LevelData GetLevelWithRepeatingLogic(int index)
        {
            if (index < NumberOfTotalLevels)
            {
                return LevelFunnelLibrary.GetLevel(index);
            }
            else
            {
                return LevelFunnelLibrary.GetLevel(GetRepeatingLevelIndex());
            }
        }


        protected virtual void LoadLevelWithData(LevelData levelData)
        {
            ActiveLevelData = levelData;
            //Try to instantiate level prefab if exists
            if (levelData.LevelPrefab)
            {
                LoadLevelPrefab(levelData.LevelPrefab);
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
                GlobalActions.OnLevelChanged -= OnLevelChanged;
                GlobalActions.OnGameRestarted -= ReloadLevel;
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
                stats += "Active Level: " + ActiveLevelData.LevelName;
            }

            return stats;
        }
    }
}
