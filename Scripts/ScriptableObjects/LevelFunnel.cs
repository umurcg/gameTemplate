using CorePublic.Helpers;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelFunnel", menuName = "Reboot/LevelFunnel", order = 1)]
    public class LevelFunnel : GlobalSO<LevelFunnel>
    {
        public bool RandomizeLevelsAfterFunnelFinished = false;
        public int DefaultRepeatStartLevelIndex = -1;

        public LevelData[] Levels;

        public LevelData GetLevel(int index)
        {
            return Levels[index];
        }

        protected override void LoadSettings()
        {
            var json = RemoteConfig.Instance.GetJson(RemoteConfigKey);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Level funnel is not set");
                return;
            }

            //Just get RandomizeLevelsAfterFunnelFinished and DefaultRepeatStartLevelIndex from json
            var remoteFunnel = JsonUtility.FromJson<LevelFunnel>(json);
            if (remoteFunnel == null)
            {
                Debug.LogError("Level funnel library is not valid");
                return;
            }
            RandomizeLevelsAfterFunnelFinished = remoteFunnel.RandomizeLevelsAfterFunnelFinished;
            DefaultRepeatStartLevelIndex = remoteFunnel.DefaultRepeatStartLevelIndex;
        }
    }
}
