using ActionTriggers;
using UnityEditor;

namespace Core.Editor
{
    [CustomEditor(typeof(ActionTrigger))]
    public class ActionTriggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var script = target as ActionTrigger;
            
            if (script.ShowEventDescription != null && script.Descriptions.ContainsKey(script.ShowEventDescription))
            {
                var description = script.Descriptions[script.ShowEventDescription];
                EditorGUILayout.HelpBox(description, MessageType.Info);
            }
        }
    }
}
