
using Core;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public abstract class Touch : MonoBehaviour
    {
        private bool _inputListenerAssigned;

        public enum ScreenAreas
        {
            All,
            Right,
            Left
        }

        // Start is called before the first frame update
        void Start()
        {
            var touchEnabled = RemoteConfig.Instance.GetBool("touchToolsEnabled", true);
            if (!touchEnabled)
            {
                Destroy(this);
            }
            else
            {
                LeanTouch.OnFingerDown += OnFingerDown;
                _inputListenerAssigned = true;
            }
        }

        private void OnDestroy()
        {
            if(_inputListenerAssigned) LeanTouch.OnFingerDown -= OnFingerDown;
        }

        protected abstract void OnFingerDown(LeanFinger finger);

        protected bool IsFingersInArea(ScreenAreas area)
        {
            if (area == ScreenAreas.All) return true;

            var fingers = LeanTouch.Fingers;
            foreach (LeanFinger finger in fingers)
            {
                if (finger.ScreenPosition.x < Screen.width / 2f && area == ScreenAreas.Right)
                    return false;
                if (finger.ScreenPosition.x > Screen.width / 2f && area == ScreenAreas.Left)
                    return false;
            }

            return true;
        }
    }
}
