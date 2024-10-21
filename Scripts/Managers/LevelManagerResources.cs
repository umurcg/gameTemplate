using System.Collections;
using CorePublic.ScriptableObjects;
using UnityEngine;

namespace CorePublic.Managers
{
    public class LevelManagerResources: LevelManager
    {
        public override int NumberOfTotalLevels => LevelDataNames.Length;
        public override bool LevelIsLoaded { get; }
        [SerializeField] private string levelsFolder = "Levels";
        [SerializeField]protected string[] LevelDataNames;
        
        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
            RunAutoLoad();
        }
        
        public override void LoadLevel(int levelIndex)
        {
            if (levelIndex < LevelDataNames.Length)
            {
                var level = Resources.Load<LevelData>(levelsFolder + "/" + LevelDataNames[levelIndex]);
                if (level == null)
                {
                    Debug.LogError("Level with name " + LevelDataNames[levelIndex] + " is not found in " +
                                   levelsFolder);
                    return;
                }
                
                LoadLevel(level);
            }
            else
            {
                LoadRepeatLevel();
            }
        }
        
    }
}