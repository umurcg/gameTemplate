using Core.Managers;
using Lean.Touch;
using Managers;
using UI;
using UnityEngine;

namespace Core.UI
{
    public class StartUIController : UIElement
    {
        [SerializeField] private bool startWithInput;

        public override void Initialize()
        {
            base.Initialize();
            ActionManager.Instance.OnGameStarted += () => Deactivate();
            ActionManager.Instance.OnNewLevelLoaded += () => Activate();
        }

        private void OnFingerDown(LeanFinger obj)
        {
            if (!obj.IsOverGui) CoreManager.Instance.StartGame();
        }

        private void OnDestroy()
        {
            if (isActive)
            {
                LeanTouch.OnFingerDown -= OnFingerDown;
            }
            
            if (!ActionManager.Instance) return;
            ActionManager.Instance.OnNewLevelLoaded -= (Activate);
            ActionManager.Instance.OnGameStarted -= (Deactivate);

     
        }

        public override void SetActive(bool activate)
        {
            base.SetActive(activate);
            if (activate)
            {
                LeanTouch.OnFingerDown += OnFingerDown;
            }
            else
            {
                LeanTouch.OnFingerDown -= OnFingerDown;
            }
        }
    }
}