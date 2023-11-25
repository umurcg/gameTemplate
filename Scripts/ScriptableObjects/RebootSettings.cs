
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    public class RebootSettings : ScriptableObject
    {
        public string gameName;
        public string bundleID;
        public string bundleVersion = "0.0.01";
        public int bundleVersionCode;
        public string remoteConfigEnvironmentID;
        

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
        }

        public bool ReadyToBuild(out string error)
        {
            error = "";
            bool fieldsFull = gameName != "" && bundleID != "" && bundleVersionCode >= 0 && remoteConfigEnvironmentID != "";
            if (!fieldsFull)
            {
                error="Reboot settings fields are not fullfilled";
                return false;
            }

            if (PlayerSettings.companyName != "Reboot Interactive" ||
                PlayerSettings.productName != gameName ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) != bundleID ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android) != bundleID ||
                PlayerSettings.Android.bundleVersionCode != bundleVersionCode ||
                PlayerSettings.iOS.buildNumber != bundleVersionCode.ToString())
            {
                error= "Reboot settings fields are not synced with player settings!";
                return false;
            }

            return true;
        }
#endif
    }
}
