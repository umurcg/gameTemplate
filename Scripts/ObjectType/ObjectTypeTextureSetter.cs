using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeTextureSetter:MonoBehaviour,IObjectTypeListener
    {
        public int textureIndex;
        
        public void OnObjectTypeChanged(Type type)
        {
            if (type.textures.Length > textureIndex)
            {
                var rend = GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.mainTexture = type.textures[textureIndex];
                }
            }
            else
            {
                Debug.LogError("Texture index out of range");
            }
        }
        
    }
}