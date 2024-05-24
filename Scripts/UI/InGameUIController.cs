using Core.UI;
using Managers;

namespace UI
{
    public class InGameUIController : UIElement
    {
        public override void Initialize()
        {
            base.Initialize();

            GlobalActions.OnGameStarted+=Activate;
            GlobalActions.OnGameLost+=Deactivate;
            GlobalActions.OnGameWin+=Deactivate;

            if (CoreManager.IsGameStarted)
                Activate();
        }

        private void OnDestroy()
        {
            GlobalActions.OnGameStarted-=Activate;
            GlobalActions.OnGameLost-=Deactivate;
            GlobalActions.OnGameWin-=Deactivate;
        }
    }
}