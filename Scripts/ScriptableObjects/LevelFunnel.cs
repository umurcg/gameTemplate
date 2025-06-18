using CorePublic.Helpers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelFunnel", menuName = "Reboot/LevelFunnel", order = 1)]
    public class LevelFunnel : GlobalSO<LevelFunnel>
    {
        public LevelData[] Levels;

        public LevelData GetLevel(int index)
        {
            return Levels[index];
        }
    }
}
