using CorePublic.Managers;

namespace CorePublic.UI
{
    public class NextLevelButton : CoreButton
    {
        public override void OnCall()
        {
            base.OnCall();
            CoreManager.Instance.IncreaseLevel();
        }
    }
}
