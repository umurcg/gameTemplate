using Managers;
using UI;

namespace Puzzle.UI
{
    public class LevelTextField : TextController
    {
        public int offset=1;

        private void Start()
        {
            ActionManager.Instance.OnLevelChanged+=LevelIsChanged;
            LevelIsChanged(CoreManager.Instance.Level);
        }

        private void OnDestroy()
        {
            if(!ActionManager.Instance) return;
            ActionManager.Instance.OnLevelChanged-=LevelIsChanged;
        }

        private void LevelIsChanged(int level)
        {
            level+=offset;
            SetText(level.ToString());
        }
    }
}
