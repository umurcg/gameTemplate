using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.UI
{
    public class CanvasGroupEvent : MonoBehaviour, ICanvasGroupListener
    {
        public UnityEvent OnActivateEvent;
        public UnityEvent OnDeactivateEvent;
        
        public void OnActivate()
        {
            OnActivateEvent.Invoke();
        }

        public void OnDeactivate()
        {
            OnDeactivateEvent.Invoke();
        }
    }
}
