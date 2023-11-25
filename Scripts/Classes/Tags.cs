using System;
using UnityEngine;

namespace Classes
{
    [Serializable]public class Tags
    {
        public Tag[] tags;
        
        public bool Contains(Tag tag)
        {
            return Array.Exists(tags, t => t.Equals(tag));
        }
        
        public bool Contains(string tag)
        {
            return Array.Exists(tags, t => t.Equals(tag));
        }
        
        public bool Contains(GameObject obj)
        {
            return Array.Exists(tags, t => t.Equals(obj));
        }
        
        public bool Contains(Collider obj)
        {
            return Array.Exists(tags, t => t.Equals(obj));
        }
    }
}