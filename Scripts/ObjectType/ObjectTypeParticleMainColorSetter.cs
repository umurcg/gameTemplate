using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeParticleMainColorSetter:MonoBehaviour,IObjectTypeListener
    {
        public int materialIndex;
        public string colorName = "_MainColor";
        
        public void OnObjectTypeChanged(Type type)
        {
            if (type.materials.Length > materialIndex)
            {
                var rend = GetComponent<ParticleSystemRenderer>();
                if (rend != null)
                {
                    rend.material.SetColor(colorName,type.colors[materialIndex]);
                }
            }
            else
            {
                Debug.LogError("Material index out of range");
            }
        }
        
    }
    
}