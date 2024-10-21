using System.Collections;
using System.Collections.Generic;
using CorePublic.Classes;
using CorePublic.ScriptableObjects;
using UnityEngine;

namespace CorePublic.Managers
{
    public class LevelManagerLevelData: LevelManager
    {
        [SerializeField]protected LevelData[] Levels;
        [SerializeField]protected LevelData[] AllLevels;
        
#if UNITY_EDITOR
        public LevelData TestLevelData;
#endif
        
        public LevelData ActiveLevelData { get; protected set; }


        public override int NumberOfTotalLevels => Levels.Length;
        public override bool LevelIsLoaded => ActiveLevelData != null;
        
        protected override IEnumerator Start()
        {
            RetrieveRemoteFunnel();
            yield return StartCoroutine(base.Start());
            
            if(RunDesignModeLoad()) yield break;
            
                        
#if UNITY_EDITOR
            
            if (TestLevelData)
            {
                LoadLevel(TestLevelData);
                GlobalActions.OnGameWin += GameWin;
                yield break;
            }
#endif
            
            RunAutoLoad();
            
        }

        public override void LoadLevel(int levelIndex)
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
                LevelData level = Levels[levelIndex];
                LoadLevel(level);
            }
            else
            {
                LoadRepeatLevel();
            }
            
        }

        private void RetrieveRemoteFunnel()
        {
            if (RemoteConfig.Instance==null) return;
            string levelKeysJson = RemoteConfig.Instance.GetJson("levelFunnel");
            var remote = JsonUtility.FromJson<LevelRemoteWrapper>(levelKeysJson);

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
        }
        
#if UNITY_EDITOR
        public void LoadTestLevel()
        {
            LoadLevel(TestLevelData);
        }
#endif



    }
}