using System;
using UnityEngine;

namespace CorePublic.Helpers
{
    public abstract class LayoutController: MonoBehaviour
    {
        public enum RuntimeUpdateMode
        {
            NoUpdate,
            UpdateOnChildrenCountChange,
            AlwaysUpdate
        }
        
        [SerializeField] public RuntimeUpdateMode runtimeUpdateMode;
        [SerializeField] public bool updateOnAwake;
        [SerializeField] public bool updateOnStart;
        [SerializeField] public bool updateOnValidate;
        [SerializeField] public bool destroyOnPlay = true;
        
        public abstract void UpdateChildrenPosition();
        protected int LastChildrenCount;
        
        
        

        protected virtual void Awake()
        {
            if (destroyOnPlay)
            {
                Destroy(this);
                return;
            }
            
            LastChildrenCount = transform.childCount;
            if (updateOnAwake)
            {
                UpdateChildrenPosition();
            }
        }
        
        protected virtual void Start()
        {
            if (updateOnStart)
            {
                UpdateChildrenPosition();
            }
        }

        protected virtual void Update()
        {
            if (runtimeUpdateMode == RuntimeUpdateMode.UpdateOnChildrenCountChange)
            {
                if (LastChildrenCount != transform.childCount)
                {
                    UpdateChildrenPosition();
                    LastChildrenCount = transform.childCount;
                }
            }
            else if (runtimeUpdateMode == RuntimeUpdateMode.AlwaysUpdate)
            {
                UpdateChildrenPosition();
            }
        }
        
        protected virtual void OnValidate()
        {
            if (updateOnValidate)
            {
                UpdateChildrenPosition();
            }
        }
     
    }
}