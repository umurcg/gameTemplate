using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Puzzle.UI
{
    [RequireComponent(typeof(Button))]
    public class FadeButton : MonoBehaviour
    {
        private Button _button;
        public UnityEvent onFadeInComplete;

        private void Start()
        {
            _button = GetComponent<Button>();
            if (_button)
                _button.onClick.AddListener(FadeInOut);
        }

        public void FadeInOut()
        {
            _button.interactable = false;
            FadeScreen.Instance.FadeIn(() =>
            {
                onFadeInComplete?.Invoke();
                _button.interactable = true;
                FadeScreen.Instance.FadeOut(null);
            });
        }
    }
}