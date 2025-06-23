using CorePublic.Helpers;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelFunnelLibrary", menuName = "Reboot/LevelFunnelLibrary", order = 1)]
    public class LevelFunnelLibrary : ScriptableObject
    {
        public string FunnelIndexRemoteConfigKey = "level_funnel_index";
        public int LevelFunnelIndex = 0;
        public LevelFunnel[] LevelFunnels;


        public LevelFunnel GetCurrentLevelFunnel()
        {
            return GetLevelFunnel(LevelFunnelIndex);
        }

        public LevelFunnel GetLevelFunnel(int index)
        {
            if (LevelFunnels == null || LevelFunnels.Length == 0)
            {
                Debug.LogError("Level funnels are not valid");
                return null;
            }
            
            if (index < 0 || index >= LevelFunnels.Length)
            {
                Debug.LogError($"Level funnel index is out of range: {index}");
                return null;
            }
            return LevelFunnels[index];
        }

        public int GetNumberOfTotalLevelsOfCurrentFunnel()
        {
            var levelFunnel = GetCurrentLevelFunnel();
            if (levelFunnel == null)
            {
                Debug.LogError("Default level funnel is not valid");
                return 0;
            }
            return levelFunnel.Levels.Length;
        }

        public LevelData GetLevel(int index)
        {
            var levelFunnel = GetCurrentLevelFunnel();
            if (levelFunnel == null)
            {
                Debug.LogError("Default level funnel is not valid");
                return null;
            }
            return levelFunnel.GetLevel(index);
        }

        public LevelFunnelLibrary LoadRemoteSettings()
        {
            if (RemoteConfig.Instance)
            {
                //Create a new instance of the scriptable object to avoid modifying the original one
                var newInstance = CreateInstance<LevelFunnelLibrary>();
                newInstance.LevelFunnelIndex = RemoteConfig.Instance.GetInt(FunnelIndexRemoteConfigKey, LevelFunnelIndex);
                return newInstance;
            }
            return this;
        }
    }
}