using Core.UI;
using Managers;

namespace UI
{
    /// <summary>
    ///     Retrieve current level from game controller and assign it to text component of gameObject
    /// </summary>
    public class LevelText : TextController
    {
        private CoreManager _coreManager;

        private void Start()
        {
            _coreManager = CoreManager.Request();

            if (_coreManager == null) return;
            GlobalActions.OnLevelChanged+=LevelIsChanged;

            LevelIsChanged(_coreManager.Level);
        }

        private void OnDestroy()
        {
            GlobalActions.OnLevelChanged-=LevelIsChanged;
        }

        private void LevelIsChanged(int level)
        {
            level++;
            SetText(level.ToString());
        }
    }
}