using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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