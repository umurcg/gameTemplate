using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelDataCreator
{
    [MenuItem("Assets/Create/LevelData", false, 10)]
    private static void CreateLevelData()
    {
        GameObject selectedPrefab = Selection.activeObject as GameObject;
        if (selectedPrefab == null)
        {
            Debug.LogError("Please select a prefab to create LevelData.");
            return;
        }

        string folderPath = "Assets/Resources/LevelData";
        string alternativePath = "Assets/Levels";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            if (AssetDatabase.IsValidFolder(alternativePath))
            {
                folderPath = alternativePath;
                Debug.Log($"Using alternative folder path: {folderPath}");
            }
            else
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.CreateFolder("Assets/Resources", "Levels");
                Debug.Log($"Created folder path: {folderPath}");
            }
        }

        string assetPath = $"{folderPath}/{selectedPrefab.name}.asset";
        ScriptableObjects.LevelData levelData = ScriptableObject.CreateInstance<ScriptableObjects.LevelData>();
        levelData.levelPrefab = selectedPrefab;

        AssetDatabase.CreateAsset(levelData, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Created LevelData asset at {assetPath}");

        Selection.activeObject = levelData;
    }

    [MenuItem("Assets/Create/LevelData", true)]
    private static bool ValidateLevelDataCreation()
    {
        return Selection.activeObject is GameObject;
    }
}