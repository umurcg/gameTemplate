using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    public class EditorCodeChecker
    {
        [MenuItem("Reboot/Tools/Check Editor Codes")]
        public static void CheckForEditorUsage()
        {
            // Get all scripts in the project
            string[] allScripts = AssetDatabase.FindAssets("t:Script");

            bool warning = false;

            // Loop through all scripts
            foreach (string scriptGUID in allScripts)
            {
                string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                
                //If the script is in the editor folder, skip it
                if (scriptPath.Contains("Editor"))
                    continue;

                //If the script is in the plugins folder, skip it
                if (scriptPath.Contains("Plugins"))
                    continue;

                //If the script is not mono, skip it
                if (!scriptPath.Contains(".cs"))
                    continue;
                
                //If the script or any of its parent is scriptable object, skip it
                var script = AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptPath);
                if(script!=null)
                    continue;
              
                //If unity package, skip it
                if (scriptPath.Contains("Packages/com.unity"))
                    continue;
                
                
                // Read the contents of the script
                string scriptText = File.ReadAllText(scriptPath);

                List<Vector2Int> preprocessorDirectiveLocations = new List<Vector2Int>();

                //Get lines of #if UNITY_EDITOR
                string[] lines = scriptText.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("#if UNITY_EDITOR"))
                    {
                        //Find following #endif
                        for (int j = i; j < lines.Length; j++)
                        {
                            if (lines[j].Contains("#endif"))
                            {
                                preprocessorDirectiveLocations.Add(new Vector2Int(i, j));
                                break;
                            }
                        }
                    }
                }

                //If there are UnityEditor usages outside of #if UNITY_EDITOR preprocessor directives then log a warning
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("UnityEditor"))
                    {
                        bool isInsidePreprocessorDirective = false;
                        foreach (Vector2Int preprocessorDirectiveLocation in preprocessorDirectiveLocations)
                        {
                            if (i >= preprocessorDirectiveLocation.x && i <= preprocessorDirectiveLocation.y)
                            {
                                isInsidePreprocessorDirective = true;
                                break;
                            }
                        }

                        if (!isInsidePreprocessorDirective)
                        {
                            Debug.LogWarning("Script " + scriptPath +
                                             " contains EditorUtility usage outside of #if UNITY_EDITOR preprocessor directive");
                            warning = true;
                        }
                    }
                }
            }

            //If there are no warnings then log a success message in green color
            if (!warning)
            {
                Debug.Log("<color=green>Script check completed successfully</color>");
            }
        }
    }
}