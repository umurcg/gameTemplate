
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class MaterialGenerator : MonoBehaviour
{
    #if UNITY_EDITOR
    public Color[] Colors = new[]
    {
        Color.black, Color.blue, Color.cyan, Color.gray, Color.green,  Color.magenta, Color.red, Color.white,
        Color.yellow
    };
    
    public Material Material;
    
    public string path="Assets/Materials/";
    
    [ContextMenu("Generate")]
    void Generate()
    {
        if (Material == null)
            Material = new Material(Shader.Find("Standard"));

        Dictionary<Color, string> colorNames = new Dictionary<Color, string>()
        {
            {Color.black, "Black"},
            {Color.blue, "Blue"},
            {Color.cyan, "Cyan"},
            {Color.gray, "Gray"},
            {Color.green, "Green"},
            {Color.magenta, "Magenta"},
            {Color.red, "Red"},
            {Color.white, "White"},
            {Color.yellow, "Yellow"}

        };

        foreach (Color color in Colors)
        {
            Material newMaterial = new Material(Material);
            newMaterial.color = color;
            string name = colorNames.ContainsKey(color)
                ? colorNames[color]
                : color.ToString().Substring(color.ToString().Length - 28);
            Utils.SaveAsset(newMaterial, path , name);
        }
    }
    
    #endif
}
