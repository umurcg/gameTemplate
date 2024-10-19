using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class AnimationEventController: MonoBehaviour
    {
        [Serializable]public class RuntimeAnimationEvent
        {
            [HideInInspector]public string[] functionNames;
            public string name="AnimationName";
            [Range(0, 1)] public float time=1f;
            public UnityEvent onEvent;
        }
    
        public RuntimeAnimationEvent[] events;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            for (int index = 0; index < events.Length; index++)
            {
                RuntimeAnimationEvent animationEvent = events[index];
                AnimationClip originalClip = FindAnimationClip(animationEvent.name);

                // Create a new instance of the AnimationEvent class to add to the clip
                AnimationEvent newEvent = new AnimationEvent();
                newEvent.time = originalClip.length*animationEvent.time; // Set the time in seconds
                newEvent.functionName = nameof(TriggerEvent); // Set the function to call
                newEvent.intParameter = index; // Set the int parameter to pass to the function

                // Add the new AnimationEvent to the clip
                originalClip.AddEvent(newEvent);

                // Create a new AnimatorOverrideController using the modified clip
                AnimatorOverrideController overrideController = new AnimatorOverrideController();
                overrideController.runtimeAnimatorController = _animator.runtimeAnimatorController;
                overrideController.ApplyOverrides(new List<KeyValuePair<AnimationClip, AnimationClip>>()
                {
                    new KeyValuePair<AnimationClip, AnimationClip>(originalClip, originalClip)
                });

                // Set the Animator's controller to the new override controller
                _animator.runtimeAnimatorController = overrideController;
            }
        }
        
        public void TriggerEvent(int index)
        {
            events[index].onEvent?.Invoke();
        }
        
        private AnimationClip FindAnimationClip(string name)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }
            return null;
        }

        public string[] GetFunctionNames()
        {
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            List<string> functionNames = new List<string>();
            foreach (MonoBehaviour script in scripts)
            {
                System.Reflection.MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Public);
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    if (method.ReturnType == typeof(void))
                    {
                        functionNames.Add(method.Name);
                    }
                }
            }
            return functionNames.ToArray();
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var functionNames=GetFunctionNames();
            foreach (RuntimeAnimationEvent animationEvent in events)
            {
                animationEvent.functionNames = functionNames;
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}