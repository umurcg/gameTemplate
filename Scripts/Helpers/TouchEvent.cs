
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using Touch = Helpers.Touch;

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
