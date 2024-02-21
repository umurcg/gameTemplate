using System;
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
            return ColorMaterials[Random.Range(0, ColorMaterials.Length)].Color;
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
        
        //Create color library
        [ContextMenu("Create Color Library")]
        public void CreateColorLibrary()
        {
            if (ColorMaterials.Length > 0)
            {
                Debug.LogError("Color Library already created");
            }
                
            var colorTypes=Enum.GetValues(typeof(ColorObjectType));
            
            //If dont exist create material folder first
            if (!System.IO.Directory.Exists("Assets/Materials"))
            {
                System.IO.Directory.CreateDirectory("Assets/Materials");
            }
            
            //Crate base material first in Materials folder. Lit material URP
            var baseMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            baseMaterial.color = Color.white;
            UnityEditor.AssetDatabase.CreateAsset(baseMaterial, "Assets/Materials/BaseMaterial.mat");
            
            ColorMaterials=new ColorMaterial[colorTypes.Length];
            
            //Create color materials
            foreach (ColorObjectType colorType in colorTypes)
            {
                //Crate variable material
                var material = new Material(baseMaterial);
                material.color = Utility.GetColor(colorType);
                UnityEditor.AssetDatabase.CreateAsset(material, "Assets/Materials/"+colorType+".mat");
                ColorMaterials[(int)colorType]=new ColorMaterial{Color=colorType, Material=material, ColorValue=material.color};
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
            //Refresh the assets
            UnityEditor.AssetDatabase.Refresh();
        }
        
        
     
    }
}