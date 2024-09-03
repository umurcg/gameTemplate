using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupController : MonoBehaviour
    {

        protected CanvasGroup _canvasGroup;
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;

#if ODIN_INSPECTOR
[Sirenix.OdinInspector.ShowInInspector]
#endif
        public virtual void Activate()
        {
            SetEnable(true);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public virtual void Deactivate()
        {
            SetEnable(false);
        }

        public void SetEnable(bool enable)
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = enable ? 1 : 0;
            _canvasGroup.interactable = enable;
            _canvasGroup.blocksRaycasts = enable;

            if (Application.isPlaying)
            {
                if (enable)
                    OnActivate?.Invoke();
                else
                    OnDeactivate?.Invoke();
            }

            if (enable)
            {
                var parentControllers=GetComponentsInParent<CanvasGroupController>();
                foreach (var parentController in parentControllers)
                {
                    parentController.Activate();
                }
            }
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_canvasGroup);
#endif
        }
    }
}