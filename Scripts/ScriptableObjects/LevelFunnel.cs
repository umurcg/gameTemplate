using CorePublic.Helpers;

namespace CorePublic.ScriptableObjects
{
    public class LevelFunnel : GlobalSO<LevelFunnel>
    {
        public LevelData[] Levels;

        public LevelData GetLevel(int index)
        {
            return Levels[index];
        }
    }
}
