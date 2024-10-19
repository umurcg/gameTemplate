
using UnityEngine;

namespace CorePublic.Helpers
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorSetter : MonoBehaviour
    {
        public enum ParameterType
        {
            Float,
            Int,
            Boolean,
            Trigger
        }

        public string parameterName;
        public ParameterType parameterType;

        public float floatValue;
        public int intValue;
        public bool booleanValue;

        private void Awake()
        {
            var animator = GetComponent<Animator>();
            
            switch (parameterType)
            {
                case ParameterType.Float:
                    animator.SetFloat(parameterName, floatValue);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(parameterName, intValue);
                    break;
                case ParameterType.Boolean:
                    animator.SetBool(parameterName, booleanValue);
                    break;
                case ParameterType.Trigger:
                    animator.SetTrigger(parameterName);
                    break;
            }
        }

        private void OnValidate()
        {
            // You can add validation here if you want to enforce specific values or conditions
        }
    }
}
