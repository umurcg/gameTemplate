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

        public void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
            Invoke(nameof(OnCall), callDelay);
        }

        public abstract void OnCall();
    }
}