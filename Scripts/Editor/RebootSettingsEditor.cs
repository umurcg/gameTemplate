using CorePublic.ScriptableObjects;
using UnityEngine;

namespace CorePublic.Editor
{
    [UnityEditor.CustomEditor(typeof(RebootSettings))]
    public class RebootSettingsEditor: UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            RebootSettings rebootSettings = (RebootSettings) target;
            if (GUILayout.Button("Update Project Settings"))
            {
                rebootSettings.UpdateProjectSettings();
            }
        }
    }
}