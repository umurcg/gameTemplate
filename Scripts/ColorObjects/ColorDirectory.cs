using System;
using log4net.Appender;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ColorObjects
{
    [CreateAssetMenu(fileName = "ColorDirectory", menuName = "ScriptableObjects/ColorDirectory", order = 1)]
    public class ColorDirectory: ScriptableObject
    {
        public ColorMaterial[] ColorMaterials;
        
        public ColorObjectType GetRandomType()
        {
            return (ColorObjectType) Random.Range(0, Enum.GetValues(typeof(ColorObjectType)).Length);
        }
        
        public Color FindColor(ColorObjectType colorType)
        {
            //Try to load the color from the resources folder
            foreach (ColorMaterial material in ColorMaterials)
            {
                if (material.Color == colorType)
                {
                    return material.ColorValue;
                }
            }
            
            return Color.white;
        }
        
        public Material FindMaterial(ColorObjectType color)
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