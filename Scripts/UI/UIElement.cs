
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UIElement : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("UI Objects that will be activated or deactivated according to activation of the UI Element")]
        protected RectTransform[] uiObjects;

        [SerializeField] protected UIElement[] subUIElements;

        public bool initOnStart;
        public bool activateOnStart;

        [Tooltip("If true, when parent UI elements (if exists) is activate this UI element will be also activated automatically.")]
        public bool activateWithParent = true;

        [SerializeField] protected bool isInitialized;
        [SerializeField] protected bool isActive;
        private LevelManager _levelManager;
        private UIManager _uiManager;
        protected CoreManager CoreManager;
        public bool IsActive => isActive;

        public UnityEvent onInitialized;
        public UnityEvent onActivated;
        public UnityEvent onDeactivated;
        public UnityEvent<bool> onActivationChanged;

        protected virtual void Start()
        {
            if (initOnStart)
            {
                Initialize();
            }

            if (activateOnStart)
            {
                SetActive(true);
            }
        }

        public virtual void Initialize()
        {
            if (isInitialized)
            {
                Debug.LogError(gameObject.name + " is already initialized." +
                               " It is being tried to be initialized more than once which may cause unpredictable issue.");
                return;
            }
            
            isInitialized = true;

            _uiManager = UIManager.Request();
            CoreManager = CoreManager.Request();
            _levelManager = LevelManager.Request();
            
            gameObject.SetActive(true);

            foreach (var element in subUIElements)
            {
                element.Initialize();
            }

            foreach (var uiObject in uiObjects)
            {
                uiObject.gameObject.SetActive(false);
            }
            
            onInitialized?.Invoke();
        }

        [ContextMenu("Activate")]
        public virtual void Activate()
        {
            SetActive(true);
        }

        [ContextMenu("Deactivate")]
        public virtual void Deactivate()
        {
            SetActive(false);
        }

        public virtual void SetActive(bool activate)
        {
            if (!isInitialized)
            {
                Debug.LogError("UI Element " + gameObject +
                               " couldn't be activated or deactivated because it is not initialized yet.");
                return;
            }

            foreach (var uiElement in subUIElements)
            {
                if (activate && uiElement.activateWithParent)
                {
                    uiElement.SetActive(true);
                }
                else if (!activate)
                {
                    uiElement.SetActive(false);
                }
            }

            foreach (var uiObject in uiObjects)
            {
                uiObject.gameObject.SetActive(activate);
            }

            isActive = activate;
            
            onActivationChanged?.Invoke(activate);
            if (activate)
            {
                onActivated?.Invoke();
            }
            else
            {
                onDeactivated?.Invoke();
            }
        }

#if UNITY_EDITOR
        public virtual void PrepareForBuild()
        {
            gameObject.SetActive(true);
            foreach (var uiObject in uiObjects)
            {
                uiObject.gameObject.SetActive(false);
            }
            foreach (var element in subUIElements)
            {
                element.PrepareForBuild();
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
