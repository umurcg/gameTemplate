using UnityEngine;

namespace ObjectType
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ObjectTypeSpriteColorSetter:MonoBehaviour,IObjectTypeListener
    {
        public int colorIndex;

        public void OnObjectTypeChanged(Type type)
        {
            if (type.colors.Length > colorIndex)
            {
                var rend = GetComponent<SpriteRenderer>();
                if (rend != null)
                {
                    rend.color = type.colors[colorIndex];
                }
            }
            else
            {
                Debug.LogError("Color index out of range");
            }
        }
    }
    
        
    
}