using Managers;

namespace Puzzle.UI
{
    public class LostPanel: UIPanel
    {
        private bool _isReloading;
        
        public void Start()
        {
            ActionManager.Instance.OnGameLost += Activate;
            ActionManager.Instance.OnLevelDestroyed += Deactivate;
            OnDeactivated += () => _isReloading = false;
        }        
        
        
        
        public void ReloadLevel()
        {
            if (_isReloading) return;
            CoreManager.Instance.ReloadLevel();
            _isReloading = true;
        }
        
    }
}