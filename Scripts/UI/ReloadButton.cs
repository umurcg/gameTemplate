using CorePublic.Managers;

namespace CorePublic.UI
{
    
    public class ReloadButton: CoreButton
    {
        public override void OnCall()
        {
            base.OnCall();
            CoreManager.Instance.ReloadLevel();
        }
    }
}