using System.Collections;
using UnityEngine;

namespace CorePublic.Managers
{
    public class LevelManagerPrefab: LevelManager
    {
        [SerializeField] protected GameObject[] LevelPrefabs;

        public override int NumberOfTotalLevels => LevelPrefabs.Length;
        public override bool LevelIsLoaded => ActiveLevelObject != null;

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
            RunAutoLoad();
        }
        
        public override void LoadLevel(int levelIndex)
        {
            //First try to load test levels if in editor
#if UNITY_EDITOR
            if (TestLevelIndex > 0)
            {
                if (TestLevelIndex > LevelPrefabs.Length)
                {
                    Debug.LogError("Test level index is out of range of levels array");
                    return;
                }
                var levelPrefab = LevelPrefabs[TestLevelIndex - 1];
                LoadLevel(levelPrefab);
                return;
            }
#endif
         
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
}