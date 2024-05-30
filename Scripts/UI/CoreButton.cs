using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public abstract class CoreButton: MonoBehaviour
    {
        public UnityEvent onButtonPressed;
        public float callDelay = 1f;
        protected bool isPressed;

        public void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            if (isPressed) return;
            isPressed = true;
            onButtonPressed?.Invoke();
            Invoke(nameof(OnCall), callDelay);
        }

        public virtual void OnCall()
        {
            isPressed = false;
        }
    }
}