using UnityEngine;
using UnityEngine.Events;
namespace CorePublic.UI
{
    public class CanvasGroupListener: MonoBehaviour
    {
        private CanvasGroupController[] _parentCanvasGroupControllers;
        [SerializeField] private bool _checkOnStart=true;
        public UnityEvent OnActivateEvent;
        public UnityEvent OnDeactivateEvent;

        protected virtual void Start()
        {
            _parentCanvasGroupControllers = GetComponentsInParent<CanvasGroupController>();
            if (_checkOnStart){
                if (IsActive())
                    OnActivate();
                else
                    OnDeactivate();
            }
        }

        public void CheckActivation()
        {
            if (_parentCanvasGroupControllers.Length == 0)
                return;

            if (IsActive())
                OnActivate();
        }
        
        public bool IsActive(){

            foreach (var canvasGroupController in _parentCanvasGroupControllers)
            {
                if (canvasGroupController.IsActive==false)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual void OnActivate()
        {
            OnActivateEvent.Invoke();
        }
        public virtual void OnDeactivate()
        {
            OnDeactivateEvent.Invoke();
        }
        
    }
}