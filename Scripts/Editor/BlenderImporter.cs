using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    public class BlenderImporter : MonoBehaviour
    {
        [MenuItem("Assets/Import FBX with Blender")]
        private static void ImportFBXWithBlender()
        {
            // Get the selected asset path
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!assetPath.EndsWith(".fbx"))
            {
                EditorUtility.DisplayDialog("Error", "Selected file is not an FBX file.", "OK");
                return;
            }

            // Convert the Unity asset path to a full system path
            string fullPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + assetPath;
            UnityEngine.Debug.Log("Full path to FBX file: " + fullPath);

            // Define the Blender executable path
            string blenderPath = "/Applications/Blender.app/Contents/MacOS/Blender"; // Adjust this path if necessary
            UnityEngine.Debug.Log("Blender path: " + blenderPath);

            // Define the Python script content
            string pythonScript = $@"
import bpy
import sys

# Get the file path from the command line argument
fbx_file_path = sys.argv[sys.argv.index('--') + 1]
print(f'Importing FBX file from: {{fbx_file_path}}')

# Clear existing objects
print('Clearing existing objects...')
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

# Import the FBX file
print('Importing FBX...')
bpy.ops.import_scene.fbx(filepath=fbx_file_path)
print('FBX imported.')

# Check if any objects were imported
if len(bpy.context.selected_objects) == 0:
    print('No objects were imported.')
else:
    print(f'Number of objects imported: {{len(bpy.context.selected_objects)}}')

    # Apply transforms
    print('Applying transforms...')
    for obj in bpy.context.selected_objects:
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    print('Transforms applied.')

# Set the export path
bpy.context.preferences.filepaths.file_path = fbx_file_path
print(f'Set export path to: {{fbx_file_path}}')
";

            // Create a temporary Python script file
            string tempScriptPath = Path.GetTempFileName() + ".py";
            File.WriteAllText(tempScriptPath, pythonScript);
            UnityEngine.Debug.Log("Temporary script path: " + tempScriptPath);

            // Create the process start info
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = blenderPath,
                Arguments = $"--python \"{tempScriptPath}\" -- \"{fullPath}\"",
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            };
        

            // Start the process
            Process process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                // Delete the temporary Python script file
                File.Delete(tempScriptPath);
                UnityEngine.Debug.Log("Temporary script file deleted.");

                // Refresh the AssetDatabase to reflect the changes
                AssetDatabase.Refresh();
            };

            process.Start();
            UnityEngine.Debug.Log("Blender process started.");
        
            //Copy full path to clipboard of the system
            TextEditor te = new TextEditor();
            te.text = fullPath;
            te.SelectAll();
            te.Copy();
            UnityEngine.Debug.Log("Full path copied to clipboard.");
        }
    }
}