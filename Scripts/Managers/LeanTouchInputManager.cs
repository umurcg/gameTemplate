using System;
using Core.Interfaces;
using Lean.Touch;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(LeanTouch))]
    public abstract class LeanTouchInputManager : MonoBehaviour, IStats
    {
        public bool IsInputActive { get; protected set; }


        public void Start()
        {
            GlobalActions.OnGameStarted += ActivateInput;
            GlobalActions.OnGameEnded += DeactivateInput;
        }

        protected virtual void ActivateInput()
        {
            if (IsInputActive) return;
            BindEvents();
            IsInputActive = true;
        }

        protected virtual void DeactivateInput()
        {
            if (!IsInputActive) return;
            UnbindEvents();
            IsInputActive = false;
        }

        protected abstract void BindEvents();
        protected abstract void UnbindEvents();

        protected void OnDestroy()
        {
            if (IsInputActive)
                DeactivateInput();
        }

        public string GetStats()
        {
            return $"IsInputActive: {IsInputActive}";
        }
    }
}