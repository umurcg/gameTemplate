using System;
using System.Collections;
using UnityEngine;

namespace Puzzle.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        protected bool isActive;
        [SerializeField] protected bool hasActivationAnimation;
        [SerializeField] protected string animationTriggerName;
        protected Animator animator;
        public float activationDelay;
        public float deactivationDelay;
        
        public Action OnDeactivated;
        public Action OnActivated;

        protected void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Deactivate();
            animator = GetComponentInParent<Animator>();
        }
        
        public void Activate()
        {
            StartCoroutine(ActivateCoroutine());
        }
        
        protected IEnumerator ActivateCoroutine()
        {
            yield return new WaitForSeconds(activationDelay);
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isActive = true;

            if (hasActivationAnimation)
                SetAnimationTrigger();
            
            
            OnActivated?.Invoke();
        }

        public void Deactivate()
        {
            StartCoroutine(DeactivateCoroutine());
        }

        private IEnumerator DeactivateCoroutine()
        {
            yield return new WaitForSeconds(deactivationDelay);
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isActive = false;
            OnDeactivated?.Invoke();
        }
        
        
        protected void SetAnimationTrigger()
        {
            if (animator)
            {
                animator.SetTrigger(animationTriggerName);
            }
            else
            {
                Debug.LogWarning("No animator found in parent");
            }
        }
    }
}