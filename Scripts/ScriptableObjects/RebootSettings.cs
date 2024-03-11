
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    public class RebootSettings : ScriptableObject
    {
        public string gameName;
        public Texture2D gameIcon;
        public string bundleID;
        public string bundleVersion = "0.0.01";
        public int bundleVersionCode = 1;
        public string remoteConfigEnvironmentID = "xxx_rel_xxx";
        public string remoteConfigEnvironmentIDDebug = "xxx_dev_xxx";
        
        

#if UNITY_EDITOR
        [ContextMenu("Update Project Settings")]
        private void UpdateProjectSettings()
        {
            PlayerSettings.companyName="Reboot Interactive";
            PlayerSettings.productName = gameName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleID);
            PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
            PlayerSettings.bundleVersion = bundleVersion;
            PlayerSettings.iOS.buildNumber= bundleVersionCode.ToString();
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Standalone, new[] {gameIcon});
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new[] {gameIcon});
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, new[] {gameIcon});
        }

        public bool ReadyToBuild(out string error)
        {
            error = "";
            bool fieldsFull = gameName != "" && bundleID != "" && bundleVersionCode >= 0 && remoteConfigEnvironmentID != "";
            if (!fieldsFull)
            {
                error="Reboot settings fields are not fulfilled";
                return false;
            }

            if (PlayerSettings.companyName != "Reboot Interactive" ||
                PlayerSettings.productName != gameName ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) != bundleID ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android) != bundleID ||
                PlayerSettings.Android.bundleVersionCode != bundleVersionCode ||
                PlayerSettings.iOS.buildNumber != bundleVersionCode.ToString() ||
                PlayerSettings.bundleVersion != bundleVersion ||
                PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Standalone)[0] != gameIcon ||
                PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Android)[0] != gameIcon ||
                PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.iOS)[0] != gameIcon)
            {
                error= "Reboot settings fields are not synced with player settings!";
                return false;
            }

            return true;
        }
#endif
    }
}
