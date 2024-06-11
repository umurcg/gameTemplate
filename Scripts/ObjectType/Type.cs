using System;
using UnityEngine;

namespace ObjectType
{
    [Serializable]public struct Type
    {
        public string typeName;
        public Material[] materials;
        public Color[] colors;
        public Texture[] textures;
        public Sprite[] sprites;

        public bool IsNull()
        {
            return string.IsNullOrEmpty(typeName);
        }
    }
}