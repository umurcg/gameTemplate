using Managers;

namespace Puzzle.UI
{
    public class InGamePanel: UIPanel
    {
        public void Start()
        {
            ActionManager.Instance.OnGameStarted += Activate;
            ActionManager.Instance.OnLevelDestroyed += Deactivate;
        }
    }
}