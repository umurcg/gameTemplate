
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace ActionTriggers
{
    public abstract class BaseTrigger: MonoBehaviour
    {
        [SerializeField] public string eventName;
#if UNITY_EDITOR
        [SerializeField] private static ActionManager _actionManager;
#else
        private static ActionManager _actionManager;
#endif

        public Dictionary<string, string> Descriptions =>
            _actionManager == null ? null : _actionManager.Descriptions;
      
        // Start is called before the first frame update
        void OnValidate()
        {
            if(!_actionManager) _actionManager = FindObjectOfType<ActionManager>();
        }

        public void Start()
        {
            if(!_actionManager) _actionManager = FindObjectOfType<ActionManager>();
        
            if (!_actionManager)
            {
                Debug.LogError("GlobalEventsController not found");
                return;
            }

            var type = _actionManager.GetActionType(eventName);
            switch (type)
            {
                case ActionTrigger.Event.EventTypes.Void:
                    _actionManager.AddListenerToAction(eventName,Trigger);
                    break;
                case ActionTrigger.Event.EventTypes.Float:
                    _actionManager.AddListenerToAction<float>(eventName,Trigger_float);
                    break;
                case ActionTrigger.Event.EventTypes.Int:
                    _actionManager.AddListenerToAction<int>(eventName,Trigger_int);
                    break;
                case ActionTrigger.Event.EventTypes.Bool:
                    _actionManager.AddListenerToAction<bool>(eventName,Trigger_bool);
                    break;
                case ActionTrigger.Event.EventTypes.String:
                    _actionManager.AddListenerToAction<string>(eventName,Trigger_string);
                    break;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Trigger")]
#endif
        protected abstract void Trigger();

        private void Trigger_int(int arg0)
        {
            Trigger();
        }
        
        private void Trigger_float(float arg0)
        {
            Trigger();
        }
        
        private void Trigger_bool(bool arg0)
        {
            Trigger();
        }
        
        private void Trigger_string(string arg0)
        {
            Trigger();
        }

#if UNITY_EDITOR
        private string[] GetEvents()
        {
            if (!_actionManager) return new string[]{"No GlobalEventsController found"};
            return _actionManager.GetActionNames();
        }
#endif
    }
}
