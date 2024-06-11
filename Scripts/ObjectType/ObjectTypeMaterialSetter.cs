using UnityEngine;

namespace ObjectType
{
    [RequireComponent(typeof(Renderer))]
    public class ObjectTypeMaterialSetter : MonoBehaviour, IObjectTypeListener
    {
        public int objectTypeMaterialIndex;
        public int rendererMaterialIndex;

        public void OnObjectTypeChanged(Type type)
        {
            if (type.materials.Length > objectTypeMaterialIndex)
            {
                var rend = GetComponent<Renderer>();
                if (rend != null)
                {
#if UNITY_EDITOR

                    if (!Application.isPlaying)
                    {
                        if (rend.sharedMaterials.Length > rendererMaterialIndex)
                        {
                            var newMaterials = rend.sharedMaterials;
                            newMaterials[rendererMaterialIndex] = type.materials[objectTypeMaterialIndex];
                            rend.sharedMaterials = newMaterials;
                            return;
                        }
                      
                        Debug.LogError("Renderer shared material index out of range");
                        return;
                    }

#endif
                    if (rend.materials.Length > rendererMaterialIndex)
                    {
                        var newMaterials = rend.materials;
                        newMaterials[rendererMaterialIndex] = type.materials[objectTypeMaterialIndex];
                        rend.materials = newMaterials;
                    }
                    else
                    {
                        Debug.LogError("Renderer material index out of range");
                    }
                }
            }
            else
            {
                Debug.LogError("Material index out of range");
            }
        }
    }
}