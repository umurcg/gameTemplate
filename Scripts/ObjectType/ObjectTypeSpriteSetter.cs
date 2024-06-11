using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeSpriteSetter:MonoBehaviour,IObjectTypeListener
    {
        public int spriteIndex;
        
        public void OnObjectTypeChanged(Type type)
        {
            if (type.sprites.Length > spriteIndex)
            {
                var rend = GetComponent<SpriteRenderer>();
                if (rend != null)
                {
                    rend.sprite = type.sprites[spriteIndex];
                }
            }
            else
            {
                Debug.LogError("Sprite index out of range");
            }
        }
        
    }
    
        
    
}