using System.Collections;
using System.Collections.Generic;
using CorePublic.Classes;
using CorePublic.Helpers;
using CorePublic.Interfaces;
using CorePublic.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CorePublic.Managers
{
    public abstract class LevelManager : Singleton<LevelManager>, IStats
    {
        
        [Tooltip("If checked, level manager will load last played level automatically when game is started")]
        public bool autoLoad = true;
        
        /// <summary>
        /// If true, it means the level is loaded in design mode which means it was already in scene in editor and that level will be used as loaded level.
        /// </summary>
        public bool InDesignMode { get; private set; }

        protected CoreManager CoreManager;

        public abstract int NumberOfTotalLevels { get; }
        
        

#if UNITY_EDITOR
        public bool EnableDesignMode=true;
#endif
        
        [Tooltip("Game will be tried to start with this level ignoring game data. This is useful for fast development")]
        public int TestLevelIndex = -1;
        public int DefaultRepeatLevelIndex = -1;

        /// <summary>
        ///     Whether or not there are active loaded level in the scene
        /// </summary>
        public abstract bool LevelIsLoaded { get; }

        public GameObject ActiveLevelObject => transform.childCount > 0 ? transform.GetChild(0).gameObject : null;

        private int LastRandomLevelIndex
        {
            get => PlayerPrefs.GetInt("LastLoadedLevelIndex", -1);
            set => PlayerPrefs.SetInt("LastLoadedLevelIndex", value);
        }

        private bool LastRandomLevelWon
        {
            get => PlayerPrefs.GetInt("LastLoadedLevelWon", 0) == 1;
            set => PlayerPrefs.SetInt("LastLoadedLevelWon", value ? 1 : 0);
        }

        protected new void Awake()
        {
            base.Awake();
        }

        protected virtual bool RunDesignModeLoad()
        {
#if UNITY_EDITOR
            if (Application.isEditor && EnableDesignMode)
            {
                if (transform.childCount > 0)
                {
                    InDesignMode = true;
                    GlobalActions.OnNewLevelLoaded.Invoke();
                    return true;
                }
            }
#endif

            return false;
        }
        
        protected virtual IEnumerator Start()
        {
            CoreManager = CoreManager.Request();
            
            //Error protection
            if (NumberOfTotalLevels == 0)
            {
                Debug.LogError(
                    "There is no level assigned to level manager. You have to assign level data to run the game.");
                Destroy(gameObject);
                yield break;
            }
            
            GlobalActions.OnGameWin += GameWin;
            GlobalActions.OnGameExit += ClearLoadedLevel;

            yield return null;
            
        }

        protected virtual void RunAutoLoad()
        {
            if (autoLoad)
            {
                LoadLevel(CoreManager.Level);
                GlobalActions.OnLevelChanged += (LoadLevel);
            }
        }
        
        private void GameWin()
        {
            if (LastRandomLevelIndex >= 0) LastRandomLevelWon = true;
        }

        public abstract void LoadLevel(int levelIndex);

        protected virtual void LoadRepeatLevel()
        {
            int repeatStartLevelIndex = DefaultRepeatLevelIndex;
            if (RemoteConfig.Instance) 
                repeatStartLevelIndex=RemoteConfig.Instance.GetInt("repeatStartLevelIndex", repeatStartLevelIndex);
            
            var isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

            if (isRandomRepeating)
            {
                if (LastRandomLevelIndex > -1 && !LastRandomLevelWon)
                {
                    LoadLevel(LastRandomLevelIndex);
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
                    LoadLevel(randomLevelIndex);
                    LastRandomLevelIndex = randomLevelIndex;
                    LastRandomLevelWon = false;
                }
            }
            else
            {
                var deltaLevelIndex = CoreManager.Instance.Level - NumberOfTotalLevels;
                var repeatLevelIndex =
                    repeatStartLevelIndex +
                    deltaLevelIndex % (NumberOfTotalLevels- repeatStartLevelIndex);
                LoadLevel(repeatLevelIndex);
            }
        }


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


        //This can be edited according to needs!!
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
            if (ActiveLevelObject != null)
            {
                Destroy(ActiveLevelObject);
            }
            
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
        
        #if UNITY_EDITOR

        #if ODIN_INSPECTOR
        [Button,ShowIf(nameof(loadType), LoadType.Resource)]
        #else
        [ContextMenu("Read Levels")]
        #endif
        private void ReadLevels()
        {
            List<string> levelNames = new List<string>();
            
            var levels = Resources.LoadAll<LevelData>("Levels");
            foreach (var level in levels)
            {
                levelNames.Add(level.name);
            }
            
            LevelDataNames = levelNames.ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        #endif

#if ODIN_INSPECTOR
[Button]
#endif
        public void LoadLevelDataFromResources()
        {
            Levels = Resources.LoadAll<LevelData>("Levels");
            AllLevels = Resources.LoadAll<LevelData>("Levels");
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
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