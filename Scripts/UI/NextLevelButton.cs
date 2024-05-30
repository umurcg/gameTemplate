using Managers;
using UnityEngine;

namespace UI
{
    public class NextLevelButton : CoreButton
    {
        public override void OnCall()
        {
            CoreManager.Instance.IncreaseLevel();
        }
    }
}
