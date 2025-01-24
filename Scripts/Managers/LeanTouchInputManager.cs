using CorePublic.Interfaces;
using UnityEngine;

namespace CorePublic.Managers
{
    public abstract class LeanTouchInputManager : MonoBehaviour, IStats
    {
        public bool IsInputActive { get; protected set; }


        public virtual void Start()
        {
            GlobalActions.OnGameStarted += ActivateInput;
            GlobalActions.OnGameRevived += ActivateInput;
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

        protected virtual void OnDestroy()
        {
            if (IsInputActive)
                DeactivateInput();
        }

        public virtual string GetStats()
        {
            return $"IsInputActive: {IsInputActive}";
        }
    }
}