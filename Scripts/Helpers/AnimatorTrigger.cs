using UnityEngine;

namespace Helpers
{
    public class AnimatorTrigger : MonoBehaviour
    {
        public bool triggerOnEnable = true;
        public Animator animator;
        public string triggerName;
    
        // Start is called before the first frame update
        void OnEnable()
        {
            if (triggerOnEnable)
            {
                Trigger();
            }
        }

        public void Trigger()
        {
            if (animator)
            {
                animator.SetTrigger(triggerName);
            }
        }

        private void Reset()
        {
            animator = GetComponentInParent<Animator>();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void SetTriggerOnEnable(bool value)
        {
            triggerOnEnable = value;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
