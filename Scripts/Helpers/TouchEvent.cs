
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using Lean.Touch;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class TouchEvent : Touch
    {
        [System.Serializable] public struct Event
        {
            public int requiredTouchCount;
            public UnityEvent touchEvent;
            public ScreenAreas screenArea;
        }

        public Event[] events;

        protected override void OnFingerDown(LeanFinger finger)
        {
            foreach (Event @event in events)
            {
                if (@event.requiredTouchCount == LeanTouch.Fingers.Count && IsFingersInArea(@event.screenArea))
                    @event.touchEvent?.Invoke();
            }
        }
    }
}
