using System;
using CorePublic.Interfaces;
using UnityEngine;

namespace CorePublic.Classes
{
    [Serializable]
    public class PrerequisiteReference
    {
        public MonoBehaviour behaviour;

        public IPrerequisite Prerequisite
        {
            get
            {
                if (behaviour == null)
                {
                    Debug.LogError("PrerequisiteReference is missing a reference to a MonoBehaviour");
                    return null;
                }
                
                if (!(behaviour is IPrerequisite))
                {
                    Debug.LogError("PrerequisiteReference is referencing a MonoBehaviour that does not implement IPrerequisite");
                    return null;
                }
                return behaviour as IPrerequisite;
            }
        }
    }
}