using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabImporter : EditorWindow
{
    private string _prefabFilePath;

    [MenuItem("Tools/Import Prefab")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<PrefabImporter>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Import Prefab", EditorStyles.boldLabel);
        _prefabFilePath = EditorGUILayout.TextField("Prefab File Path", _prefabFilePath);

        if (GUILayout.Button("Import"))
        {
            if (string.IsNullOrEmpty(_prefabFilePath))
            {
                Debug.LogError("Prefab file path is empty.");
                return;
            }

            if (!File.Exists(_prefabFilePath))
            {
                Debug.LogError("Prefab file does not exist at path: " + _prefabFilePath);
                return;
            }

            string fileName = Path.GetFileName(_prefabFilePath);
            string destinationPath = Path.Combine("Assets", fileName);

            File.Copy(_prefabFilePath, destinationPath, true);

            AssetDatabase.ImportAsset(destinationPath, ImportAssetOptions.Default);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(destinationPath);

            if (prefab == null)
            {
                Debug.LogError("Prefab not found at path: " + destinationPath);
                return;
            }

            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);

            foreach (string dependency in dependencies)
            {
                if (dependency == prefabPath)
                {
                    continue;
                }

                string dependencyPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.AssetPathToGUID(dependency));
                string dependencyFileName = Path.GetFileName(dependencyPath);
                string dependencyDestinationPath = Path.Combine("Assets", dependencyFileName);

                if (!File.Exists(dependencyDestinationPath))
                {
                    File.Copy(dependencyPath, dependencyDestinationPath, true);
                    AssetDatabase.ImportAsset(dependencyDestinationPath, ImportAssetOptions.Default);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Import successful.");
        }
    }
}



// using UnityEngine;
// using UnityEditor;
// using System.IO;
//
// public class PrefabImporter
// {
//     [MenuItem("Assets/Import Prefab with Dependencies")]
//     static void ImportPrefab()
//     {
//         // Prompt the user to select the source path
//         string _prefabFilePath = EditorUtility.OpenFilePanel("Select Prefab", "", "prefab");
//         if (string.IsNullOrEmpty(_prefabFilePath))
//         {
//             Debug.LogError("No prefab selected");
//             return;
//         }
//
//
//         // Prompt the user to select the destination path
//         string destinationPath = EditorUtility.SaveFilePanel("Save Prefab", "Assets/", "prefabToImport", "prefab");
//         if (string.IsNullOrEmpty(destinationPath))
//         {
//             Debug.LogError("No destination path selected");
//             return;
//         }
//         
//         string fileName = Path.GetFileName(_prefabFilePath);
//         string tempPath = Path.Combine(Application.temporaryCachePath, fileName);
//
//         File.Copy(_prefabFilePath, tempPath, true);
//         
//         AssetDatabase.ImportAsset(tempPath, ImportAssetOptions.Default);
//
//
//         GameObject prefab = AssetDatabase.LoadAssetAtPath(tempPath, typeof(GameObject)) as GameObject;
//
//         if (prefab == null)
//         {
//             Debug.LogError("Prefab not found at path: " + tempPath);
//             return;
//         }
//
//
//         // Get dependencies
//         string prefabPath = AssetDatabase.GetAssetPath(prefab);
//         string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);
//
//         foreach (string dependency in dependencies)
//         {
//             if (dependency == prefabPath)
//             {
//                 continue;
//             }
//
//             string assetPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.AssetPathToGUID(dependency));
//             AssetDatabase.CopyAsset(assetPath, destinationPath);
//         }
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         File.Delete(tempPath);
//
//         Debug.Log("Import successful.");
//
//     }
// }