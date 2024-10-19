using Lean.Touch;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class TouchSwitcher : Touch
    {
        public Switcher[] switchers;
        
        [System.Serializable]public struct Switcher
        {
            public int requiredTouchCount;
            public GameObject switchObject;
            public ScreenAreas screenArea;
        }


        protected override void OnFingerDown(LeanFinger finger)
        {
            foreach (var switcher in switchers)
            {
                if (switcher.requiredTouchCount == LeanTouch.Fingers.Count && IsFingersInArea(switcher.screenArea))
                    switcher.switchObject.SetActive(!switcher.switchObject.activeSelf);
            }
        }
    }
}
