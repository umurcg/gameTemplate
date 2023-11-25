
using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace ActionTriggers
{
    public class ActionTrigger : MonoBehaviour
    {
        [SerializeField] private static ActionManager _actionManager;

        public Dictionary<string, string> Descriptions =>
            _actionManager == null ? null : _actionManager.Descriptions;

        [Serializable] public class Event
        {
            [SerializeField] public string eventName;

            public enum EventTypes
            {
                Void,
                Int,
                Float,
                Bool,
                String
            }

            public EventTypes eventType;
            [SerializeField] public UnityEvent _event;
            [SerializeField] public UnityEvent<int> _eventInt;
            [SerializeField] public UnityEvent<float> _eventFloat;
            [SerializeField] public UnityEvent<bool> _eventBool;
            [SerializeField] public UnityEvent<string> _eventString;
        }

        public Event[] Events;

        public string ShowEventDescription;

        private void Reset()
        {
            if(!_actionManager) _actionManager = FindObjectOfType<ActionManager>();
        
            if(Events==null) return;
            if (_actionManager)
            {
                foreach (Event @event in Events)
                {
                    @event.eventType = _actionManager.GetActionType(@event.eventName);    
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

            if(!_actionManager) _actionManager = FindObjectOfType<ActionManager>();
        
            if (!_actionManager)
            {
                Debug.LogError("GlobalEventsController not found");
                return;
            }

            foreach (Event @event in Events)
            {
                switch (@event.eventType)
                {
                    case Event.EventTypes.Void:
                        _actionManager.ConnectEventToAction(@event._event, @event.eventName);
                        break;
                    case Event.EventTypes.Int:
                        _actionManager.ConnectEventToAction(@event._eventInt, @event.eventName);
                        break;
                    case Event.EventTypes.Float:
                        _actionManager.ConnectEventToAction(@event._eventFloat, @event.eventName);
                        break;
                    case Event.EventTypes.Bool:
                        _actionManager.ConnectEventToAction(@event._eventBool, @event.eventName);
                        break;
                    case Event.EventTypes.String:
                        _actionManager.ConnectEventToAction(@event._eventString, @event.eventName);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}


// This modified version of the script removes all Odin Inspector dependencies while maintaining the same core functionality. The attributes `[ReadOnly]` and decorators like `[ValueDropdown]`, `[ShowIf]` are removed. Unity's built-in `[SerializeField]` attribute is preserved to continue to allow Unity's inspector to serialize the fields, and the logic remains the same. The `Reset` and `Start` methods currently depend on the `_actionManager` variable. Ensure that `_actionManager` is being set appropriately without the Odin Inspector functionality.