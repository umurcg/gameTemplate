using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupController : MonoBehaviour
    {
        public CanvasGroup CanvasGroup{
            get{
                if (_canvasGroup==null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        protected CanvasGroup _canvasGroup;
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

        public bool IsActive=>CanvasGroup.blocksRaycasts;

#if ODIN_INSPECTOR
[Sirenix.OdinInspector.ShowInInspector]
#else
[ContextMenu("Activate")]
#endif
        public virtual void Activate()
        {
            SetEnable(true);
        
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#else
        [ContextMenu("Deactivate")]
#endif
        public virtual void Deactivate()
        {
            SetEnable(false);            
        }
        

        public void SetEnable(bool enable)
        {
            CanvasGroup.alpha = enable ? 1 : 0;
            CanvasGroup.interactable = enable;
            CanvasGroup.blocksRaycasts = enable;

            if (Application.isPlaying)
            {
                if (enable)
                    OnActivate?.Invoke();
                else
                    OnDeactivate?.Invoke();
                
                var canvasGroupListeners = GetComponentsInChildren<CanvasGroupListener>(true);
                foreach (var listener in canvasGroupListeners)
                {
                    if (enable)
                        listener.CheckActivation();
                    else
                        listener.OnDeactivate();
                }
            }            
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(CanvasGroup);
#endif
        }
    }
}