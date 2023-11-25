using Core.UI;
using Managers;

namespace UI
{
    public class InGameUIController : UIElement
    {
        public override void Initialize()
        {
            base.Initialize();

            ActionManager.Instance.OnGameStarted+=Activate;
            ActionManager.Instance.OnGameLost+=Deactivate;
            ActionManager.Instance.OnGameWin+=Deactivate;

            if (CoreManager.IsGameStarted)
                Activate();
        }

        private void OnDestroy()
        {
            if(!ActionManager.Instance) return;
            ActionManager.Instance.OnGameStarted-=Activate;
            ActionManager.Instance.OnGameLost-=Deactivate;
            ActionManager.Instance.OnGameWin-=Deactivate;
        }
    }
}