using System;
using Cinemachine;
using UnityEngine;

namespace CorePublic.Classes
{
    [Serializable]
    public class Tag
    {
        [TagField] public string value;
        
        public bool Equals(Tag other)
        {
            return value == other.value;
        }
        
        public bool Equals(string other)
        {
            return value == other;
        }
        
        public bool Equals(GameObject obj)
        {
            return Equals(obj.tag);
        }
        
        public bool Equals(Collider obj)
        {
            return Equals(obj.gameObject);
        }
    }
}