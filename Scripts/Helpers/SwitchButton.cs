#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CorePublic.Helpers
{
    [RequireComponent(typeof(Button))]
    public class SwitchButton : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent<bool> OnSwitch;
        public UnityEvent OnSwitchOn;
        public UnityEvent OnSwitchOff;
        
        [Header("Configuration")]
        [SerializeField] protected bool defaultState = false;
        [SerializeField] protected bool defaultAndroidState = false;
        [SerializeField] protected bool defaultIOSState = false;
        [SerializeField] private bool invokeEventsAtStart;

        [Header("Persistence")]
        public bool persistent;
        
#if ODIN_INSPECTOR
        [SerializeField, ShowIf(nameof(persistent))]
        private string key = "";
#else
        [SerializeField] private string key = "";
#endif

        [Header("Visual")]
        public bool dynamicSprite;
#if ODIN_INSPECTOR
        [SerializeField, ShowIf(nameof(dynamicSprite))]
        private Sprite onTexture;

        [SerializeField, ShowIf(nameof(dynamicSprite))]
        private Sprite offTexture;
#else
        [SerializeField] private Sprite onTexture;
        [SerializeField] private Sprite offTexture;
#endif

        private Button _button;
        private Image _image;
        private bool _isOn = false;
        private bool _isInitialized = false;

        /// <summary>
        /// Current state of the switch
        /// </summary>
        public bool IsOn => _isOn;

        /// <summary>
        /// Whether the switch is interactable
        /// </summary>
        public bool Interactable 
        { 
            get => _button != null && _button.interactable;
            set 
            {
                if (_button != null)
                    _button.interactable = value;
            }
        }

        protected void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized) 
                return;

            try
            {
                // Get required components
                _button = GetComponent<Button>();
                if (_button == null)
                {
                    Debug.LogError($"[SwitchButton] Button component not found on {gameObject.name}!", this);
                    return;
                }

                // Validate persistent key
                if (persistent && string.IsNullOrEmpty(key))
                {
                    Debug.LogWarning($"[SwitchButton] Persistent is enabled but key is empty on {gameObject.name}. Using GameObject name as key.", this);
                    key = $"SwitchButton_{gameObject.name}";
                }

                // Set platform-specific default state
#if UNITY_ANDROID
                defaultState = defaultAndroidState;
#elif UNITY_IOS
                defaultState = defaultIOSState;
#endif

                // Load saved state or use default
                _isOn = persistent ? PlayerPrefs.GetInt(key, defaultState ? 1 : 0) == 1 : defaultState;

                // Setup button click listener
                _button.onClick.RemoveListener(OnClick); // Prevent duplicate listeners
                _button.onClick.AddListener(OnClick);

                // Setup image component for dynamic sprites
                if (dynamicSprite)
                {
                    _image = _button.image;
                    if (_image == null)
                    {
                        Debug.LogWarning($"[SwitchButton] Dynamic sprite enabled but no Image component found on {gameObject.name}!", this);
                        dynamicSprite = false;
                    }
                    else
                    {
                        ValidateSprites();
                        UpdateVisual();
                    }
                }

                _isInitialized = true;

                // Invoke events at start if enabled
                if (invokeEventsAtStart)
                {
                    InvokeEvents();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SwitchButton] Error during initialization on {gameObject.name}: {e.Message}", this);
            }
        }

        private void ValidateSprites()
        {
            if (onTexture == null || offTexture == null)
            {
                Debug.LogWarning($"[SwitchButton] Dynamic sprite enabled but sprites are not assigned on {gameObject.name}!", this);
            }
        }

        private void OnClick()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"[SwitchButton] OnClick called before initialization on {gameObject.name}!", this);
                return;
            }

            ToggleState();
        }

        private void ToggleState()
        {
            SetState(!_isOn, true);
        }

        private void SetState(bool newState, bool invokeEvents = true)
        {
            if (!_isInitialized)
            {
                Initialize();
                if (!_isInitialized) return;
            }

            bool stateChanged = _isOn != newState;
            _isOn = newState;

            // Update visual
            UpdateVisual();

            // Save to PlayerPrefs if persistent
            if (persistent && !string.IsNullOrEmpty(key))
            {
                try
                {
                    PlayerPrefs.SetInt(key, _isOn ? 1 : 0);
                    PlayerPrefs.Save();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[SwitchButton] Error saving persistent state for {gameObject.name}: {e.Message}", this);
                }
            }

            // Invoke events only if state actually changed or forced
            if (invokeEvents && stateChanged)
            {
                InvokeEvents();
            }
        }

        private void UpdateVisual()
        {
            if (dynamicSprite && _image != null)
            {
                Sprite targetSprite = _isOn ? onTexture : offTexture;
                if (targetSprite != null)
                {
                    _image.sprite = targetSprite;
                }
            }
        }

        private void InvokeEvents()
        {
            try
            {
                OnSwitch?.Invoke(_isOn);

                if (_isOn)
                {
                    OnSwitchOn?.Invoke();
                }
                else
                {
                    OnSwitchOff?.Invoke();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SwitchButton] Error invoking events on {gameObject.name}: {e.Message}", this);
            }
        }

        #region Public Helper Methods

        /// <summary>
        /// Sets the switch to ON state
        /// </summary>
        /// <param name="invokeEvents">Whether to invoke events when switching</param>
        public void SwitchOn(bool invokeEvents = true)
        {
            SetState(true, invokeEvents);
        }

        /// <summary>
        /// Sets the switch to OFF state
        /// </summary>
        /// <param name="invokeEvents">Whether to invoke events when switching</param>
        public void SwitchOff(bool invokeEvents = true)
        {
            SetState(false, invokeEvents);
        }

        /// <summary>
        /// Toggles the current state of the switch
        /// </summary>
        /// <param name="invokeEvents">Whether to invoke events when switching</param>
        public void Toggle(bool invokeEvents = true)
        {
            SetState(!_isOn, invokeEvents);
        }

        /// <summary>
        /// Sets the switch to a specific state
        /// </summary>
        /// <param name="state">The desired state</param>
        /// <param name="invokeEvents">Whether to invoke events when switching</param>
        public void SetSwitchState(bool state, bool invokeEvents = true)
        {
            SetState(state, invokeEvents);
        }

        /// <summary>
        /// Forces refresh of the visual state (useful after changing sprites at runtime)
        /// </summary>
        public void RefreshVisual()
        {
            UpdateVisual();
        }

        /// <summary>
        /// Resets the switch to its default state
        /// </summary>
        /// <param name="invokeEvents">Whether to invoke events when resetting</param>
        public void ResetToDefault(bool invokeEvents = true)
        {
            bool targetState = defaultState;
            
#if UNITY_ANDROID
            targetState = defaultAndroidState;
#elif UNITY_IOS
            targetState = defaultIOSState;
#endif

            SetState(targetState, invokeEvents);
        }

        /// <summary>
        /// Clears the persistent data for this switch
        /// </summary>
        public void ClearPersistentData()
        {
            if (persistent && !string.IsNullOrEmpty(key))
            {
                if (PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                    PlayerPrefs.Save();
                    Debug.Log($"[SwitchButton] Cleared persistent data for {gameObject.name}");
                }
            }
        }

        #endregion

        #region Unity Events
        
        private void OnDestroy()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(OnClick);
            }
        }

        private void OnValidate()
        {
            // Validate key naming in editor
            if (persistent && !string.IsNullOrEmpty(key))
            {
                if (key.Contains(" "))
                {
                    Debug.LogWarning($"[SwitchButton] Key contains spaces on {gameObject.name}. Consider using underscores or camelCase.", this);
                }
            }
        }

        #endregion
    }
}