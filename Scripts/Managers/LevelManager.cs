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
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CorePublic.Managers
{
    public class LevelManager : Singleton<LevelManager>, IStats
    {
        public enum LoadType
        {
            ScriptableObjectReference,
            ScriptableObjectResource,
            Prefab
        }

        public LoadType loadType = LoadType.ScriptableObjectReference;

        [Tooltip("If checked, level manager will load last played level automatically when game is started")]
        public bool autoLoad = true;

        /// <summary>
        /// If true, it means the level is loaded in design mode which means it was already in scene in editor and that level will be used as loaded level.
        /// </summary>
        public bool InDesignMode { get; private set; }

        protected CoreManager CoreManager;

        public int NumberOfTotalLevels { get; protected set; }


#if ODIN_INSPECTOR
        [ShowIf(nameof(loadType),LoadType.ScriptableObjectReference)]
#endif
        [SerializeField] protected LevelData[] Levels;

#if ODIN_INSPECTOR
        [ShowIf(nameof(loadType),LoadType.ScriptableObjectReference)]
#endif
        [SerializeField] protected LevelData[] AllLevels;

#if ODIN_INSPECTOR
        [ShowIf(nameof(loadType),LoadType.Prefab)]
#endif
        [SerializeField] protected GameObject[] LevelPrefabs;

#if ODIN_INSPECTOR
        [ShowIf(nameof(loadType),LoadType.ScriptableObjectResource)]
#endif
        [SerializeField] private string levelsFolder = "Levels";

#if ODIN_INSPECTOR
        [ShowIf(nameof(loadType), LoadType.ScriptableObjectResource)]
#endif
        [SerializeField] protected string[] LevelDataNames;

#if UNITY_EDITOR
        public LevelData TestLevelData;
        public bool EnableDesignMode = true;
#endif

        public LevelData ActiveLevelData { get; protected set; }

        [Tooltip("Game will be tried to start with this level ignoring game data. This is useful for fast development")]
        public int TestLevelIndex = -1;

        public int DefaultRepeatLevelIndex = -1;

        /// <summary>
        ///     Whether or not there are active loaded level in the scene
        /// </summary>
        public bool LevelIsLoaded => ActiveLevelData != null || ActiveLevelObject != null;

        public GameObject ActiveLevelObject => transform.childCount > 0 ? transform.GetChild(0).gameObject : null;

        [SerializeField] private string levelFunnelRemoteKey = "levelFunnel";

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

        public int RepeatStartLevelIndex
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

        protected new void Awake()
        {
            base.Awake();
        }

        protected IEnumerator Start()
        {
            CoreManager = CoreManager.Request();

            if (loadType == LoadType.ScriptableObjectReference){
                RetrieveRemoteFunnel();
            }

            if (loadType == LoadType.ScriptableObjectReference)
            {
                NumberOfTotalLevels = Levels.Length;
            }
            else if (loadType == LoadType.Prefab)
            {
                NumberOfTotalLevels = LevelPrefabs.Length;
            }
            else
            {
                NumberOfTotalLevels = LevelDataNames.Length;
            }


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

            if (loadType == LoadType.ScriptableObjectReference && TestLevelData)
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

        private void RetrieveRemoteFunnel()
        {
            if (RemoteConfig.Instance == null) return;
            if (RemoteConfig.Instance.HasKey(levelFunnelRemoteKey) == false) return;

            string levelKeysJson = RemoteConfig.Instance.GetJson(levelFunnelRemoteKey);
            var remote = JsonUtility.FromJson<LevelRemoteWrapper>(levelKeysJson);
            if (remote == null)
            {
                Debug.LogError("Remote level funnel is not defined correctly");
                return;
            }

            var key_levels = new Dictionary<string, LevelData>();
            foreach (var level in AllLevels)
            {
                var levelKey = level.levelName;
                if (levelKey == "")
                {
                    Debug.LogError("There is not defined key for level " + level.name);
                    return;
                }

                key_levels.Add(levelKey, level);
            }

            var remoteLevels = new List<LevelData>();
            foreach (var key in remote.levelKeys)
            {
                if (key_levels.ContainsKey(key) == false)
                {
                    Debug.LogWarning("There is no level defined with key " + key);
                    return;
                }

                remoteLevels.Add(key_levels[key]);
            }

            Levels = remoteLevels.ToArray();

            string log="Remote level funnel:";
            foreach(var level in Levels){
                log+=level.levelName+", ";
            }
            Debug.Log(log);
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
                if (loadType == LoadType.ScriptableObjectReference)
                {
                    if (TestLevelIndex > Levels.Length)
                    {
                        Debug.LogError("Test level index is out of range of levels array");
                        return;
                    }

                    var level = Levels[TestLevelIndex - 1];
                    LoadLevel(level);
                }
                else if (loadType == LoadType.Prefab)
                {
                    if (TestLevelIndex > LevelPrefabs.Length)
                    {
                        Debug.LogError("Test level index is out of range of levels array");
                        return;
                    }

                    var levelPrefab = LevelPrefabs[TestLevelIndex - 1];
                    LoadLevel(levelPrefab);
                }

                return;
            }
#endif

            if (loadType is LoadType.ScriptableObjectReference or LoadType.ScriptableObjectResource)
            {
                //If level index is in range of level array capacity
                if (levelIndex < Levels.Length)
                {
                    LevelData level;
                    if (loadType == LoadType.ScriptableObjectReference)
                    {
                        level = Levels[levelIndex];
                    }
                    else
                    {
                        level = Resources.Load<LevelData>(levelsFolder + "/" + LevelDataNames[levelIndex]);
                        if (level == null)
                        {
                            Debug.LogError("Level with name " + LevelDataNames[levelIndex] + " is not found in " +
                                           levelsFolder);
                            return;
                        }
                    }

                    LoadLevel(level);
                }
                else
                {
                    LoadRepeatLevel();
                }
            }
            else if (loadType == LoadType.Prefab)
            {
                if (levelIndex < LevelPrefabs.Length)
                {
                    LoadLevel(LevelPrefabs[levelIndex]);
                }
                else
                {
                    LoadRepeatLevel();
                }
            }
        }

        public int GetNextLevelIndex(int levelIndex)
        {
            if(levelIndex<NumberOfTotalLevels-1){
                return levelIndex;
            }
            else{
                return GetRepeatingLevelIndex();
            }
        }
        protected virtual void LoadRepeatLevel()
        {
            int repeatLevelIndex = GetRepeatingLevelIndex(); 
        
            int repeatStartLevelIndex = RepeatStartLevelIndex;
            var isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

            if (isRandomRepeating)
            {
                if (LastRandomLevelIndex < 0 || LastRandomLevelWon)
                {
                    LastRandomLevelIndex = repeatLevelIndex;
                    LastRandomLevelWon = false;
                }              
            }   

            LoadLevel(repeatLevelIndex);        
            
        }

        public int GetRepeatingLevelIndex(){
            
            int repeatStartLevelIndex = RepeatStartLevelIndex;
            bool isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

             if (isRandomRepeating)
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

#if ODIN_INSPECTOR
    [Button]
#else
        [ContextMenu("GetCurrentLevelOccurenceCount")]
#endif
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
            if(index < Levels.Length){
                return Levels[index];
            }
            else{
                
                int repeatStartLevelIndex = RepeatStartLevelIndex;
                var isRandomRepeating = repeatStartLevelIndex == -1 || repeatStartLevelIndex >= NumberOfTotalLevels;

                if(isRandomRepeating) return null;

                var deltaLevelIndex = CoreManager.Instance.Level - NumberOfTotalLevels;
                var repeatLevelIndex =
                    repeatStartLevelIndex +
                    deltaLevelIndex % (NumberOfTotalLevels - repeatStartLevelIndex);
                return Levels[repeatLevelIndex];
            }
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
        [Button,ShowIf(nameof(loadType), LoadType.ScriptableObjectResource)]
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
#else
        [ContextMenu("Load Level Data From Resources")]
#endif
        public void LoadLevelDataFromResources()
        {
            Levels = Resources.LoadAll<LevelData>("Levels");
            AllLevels = Resources.LoadAll<LevelData>("Levels");
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }


#if UNITY_EDITOR
        #if ODIN_INSPECTOR
        [Button]
        #else
        [ContextMenu("Log Current Level Funnel")]
        #endif
        public void LogCurrentLevelFunnel()
        {
            //Debug log current level funnel as json for remote input
            var wrapper = new LevelRemoteWrapper
            {
                levelKeys = Levels.Select(level => level.levelName).ToArray()
            };
            
            string json = JsonUtility.ToJson(wrapper, true);
            Debug.Log($"Current Level Funnel JSON for Remote Config key '{levelFunnelRemoteKey}':\n{json}");
            
            // Save to file
            string folderPath = "Assets/RemoteConfigs";
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            
            string filePath = $"{folderPath}/level_funnel.json";
            System.IO.File.WriteAllText(filePath, json);
            UnityEditor.AssetDatabase.Refresh();
            
            Debug.Log($"Saved level funnel JSON to: {filePath}");
        }
#endif


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