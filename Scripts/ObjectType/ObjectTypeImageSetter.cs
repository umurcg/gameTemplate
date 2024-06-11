using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeImageSetter:MonoBehaviour,IObjectTypeListener
    {
        public int spriteIndex;
        
        public void OnObjectTypeChanged(Type type)
        {
            if (type.sprites.Length > spriteIndex)
            {
                var image = GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.sprite = type.sprites[spriteIndex];
                }
            }
            else
            {
                Debug.LogError("Image index out of range");
            }
        }
    }
}