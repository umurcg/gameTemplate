using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Build;
#endif
using UnityEngine;


namespace CorePublic.ScriptableObjects
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

        public void UpdateProjectSettings()
        {
            PlayerSettings.companyName = "Reboot Interactive";
            PlayerSettings.productName = gameName;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleID);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleID);
            PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
            PlayerSettings.bundleVersion = bundleVersion;
            PlayerSettings.iOS.buildNumber = bundleVersionCode.ToString();
            PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { gameIcon }, IconKind.Any);

            Debug.Log("Project settings updated");
        }

        public bool ReadyToBuild(out string error)
        {
            error = "";
            bool fieldsFull = gameName != "" && bundleID != "" && bundleVersionCode >= 0 &&
                              remoteConfigEnvironmentID != "";
            if (!fieldsFull)
            {
                error = "Reboot settings fields are not fulfilled";
                return false;
            }

            if (PlayerSettings.companyName != "Reboot Interactive" ||
                PlayerSettings.productName != gameName ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) != bundleID ||
                PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android) != bundleID ||
                PlayerSettings.Android.bundleVersionCode != bundleVersionCode ||
                PlayerSettings.iOS.buildNumber != bundleVersionCode.ToString() ||
                PlayerSettings.bundleVersion != bundleVersion ||
                PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown)[0] != gameIcon)
            {
                error = "Reboot settings fields are not synced with player settings!";
                return false;
            }

            return true;
        }
#endif
    }
}