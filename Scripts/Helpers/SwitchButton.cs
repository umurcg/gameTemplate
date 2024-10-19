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
        public UnityEvent<bool> OnSwitch;
        public UnityEvent OnSwitchOn;
        public UnityEvent OnSwitchOff;
        private Button _button;
        private bool _isOn = false;

        [SerializeField] protected bool defaultState = false;
        [SerializeField] protected bool defaultAndroidState = false;
        [SerializeField] protected bool defaultIOSState = false;
        [SerializeField] private bool invokeEventsAtStart;

        public bool persistent;
#if ODIN_INSPECTOR
    [SerializeField, ShowIf(nameof(persistent))]
    private string key;
#else
        [SerializeField] private string key;
#endif

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

        private Image _image;

        // Start is called before the first frame update
        protected void Start()
        {
        
#if UNITY_ANDROID
            defaultState = defaultAndroidState;
#elif UNITY_IOS
        defaultState = defaultIOSState;
#endif
        
            _isOn = persistent ? PlayerPrefs.GetInt(key, defaultState ? 1 : 0) == 1 : defaultState;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);

            _image = _button.image;
            if (dynamicSprite) _image.sprite = _isOn ? onTexture : offTexture;

            if (invokeEventsAtStart)
            {
                if (_isOn)
                {
                    OnSwitchOn.Invoke();
                }
                else
                {
                    OnSwitchOff.Invoke();
                }

                OnSwitch.Invoke(_isOn);
            }
        }

        private void OnClick()
        {
            _isOn = !_isOn;
            OnSwitch.Invoke(_isOn);

            if (_isOn)
            {
                OnSwitchOn.Invoke();
            }
            else
            {
                OnSwitchOff.Invoke();
            }

            if (dynamicSprite) _image.sprite = _isOn ? onTexture : offTexture;
            if (persistent) PlayerPrefs.SetInt(key, _isOn ? 1 : 0);
        }
    }
}