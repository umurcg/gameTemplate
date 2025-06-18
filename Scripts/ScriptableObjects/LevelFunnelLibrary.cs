using CorePublic.Helpers;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    public class LevelFunnelLibrary : GlobalSO<LevelFunnelLibrary>
    {
        public new string RemoteConfigKey = "level_funnel_library";
        public int DefaultLevelFunnelIndex = 0;
        public LevelFunnel[] LevelFunnels;

        public LevelFunnel GetLevelFunnel(int index)
        {
            if (index < 0 || index >= LevelFunnels.Length)
            {
                Debug.LogError($"Level funnel index is out of range: {index}");
                return null;
            }
            return LevelFunnels[index];
        }
        
        public int GetNumberOfTotalLevelsOfCurrentFunnel()
        {
            var levelFunnel = GetLevelFunnel(DefaultLevelFunnelIndex);
            if (levelFunnel == null)
            {
                Debug.LogError("Default level funnel is not valid");
                return 0;
            }
            return levelFunnel.Levels.Length;
        }

        public LevelData GetLevel(int index)
        {
            var levelFunnel = GetLevelFunnel(DefaultLevelFunnelIndex);
            if (levelFunnel == null)
            {
                Debug.LogError("Default level funnel is not valid");
                return null;
            }
            return levelFunnel.GetLevel(index);
        }

        protected override void LoadSettings(){
            var json = RemoteConfig.Instance.GetJson(RemoteConfigKey);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Level funnel library is not set");
                return;
            }

            //Just get DefaultLevelFunnelIndex from json
            var remoteFunnelLibrary = JsonUtility.FromJson<LevelFunnelLibrary>(json);
            if (remoteFunnelLibrary == null)
            {
                Debug.LogError("Level funnel library is not valid");
                return;
            }
            DefaultLevelFunnelIndex = remoteFunnelLibrary.DefaultLevelFunnelIndex;
            
        }
        
    }
}