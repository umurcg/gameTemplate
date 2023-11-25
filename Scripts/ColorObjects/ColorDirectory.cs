using UnityEngine;

namespace ColorObjects
{
    [CreateAssetMenu(fileName = "ColorDirectory", menuName = "ScriptableObjects/ColorDirectory", order = 1)]
    public class ColorDirectory: ScriptableObject
    {
        public ColorMaterial[] ColorMaterials;
        
        public Material FindMaterial(Colors color)
        {
            //Try to load the material from the resources folder
            foreach (ColorMaterial material in ColorMaterials)
            {
                if (material.Color == color)
                {
                    return material.Material;
                }
            }
            
            return null;
        }
    }
}