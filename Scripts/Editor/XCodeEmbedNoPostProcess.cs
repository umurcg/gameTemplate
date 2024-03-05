#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class XCodeEmbedNoPostProcess
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        Debug.Log("Postprocess script started.");

        if (buildTarget == BuildTarget.iOS)
        {
            Debug.Log("iOS build detected.");

            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            // Targets
            string mainTargetGuid = proj.GetUnityMainTargetGuid();
            string frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();

            string[] targetGuids = { mainTargetGuid, frameworkTargetGuid };

            foreach (string targetGuid in targetGuids)
            {
                proj.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
                Debug.Log($"Set ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES to NO for target GUID '{targetGuid}'.");
            }

            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
}
#endif