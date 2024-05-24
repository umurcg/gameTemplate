using Managers;

namespace Puzzle.UI
{
    public class InGamePanel: UIPanel
    {
        public void Start()
        {
            GlobalActions.OnGameStarted += Activate;
            GlobalActions.OnLevelDestroyed += Deactivate;
        }
    }
}