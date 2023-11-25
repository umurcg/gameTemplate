
using ActionTriggers;
using Core.ActionTriggers;
using UnityEditor;

namespace Core.Editor
{
    [CustomEditor(typeof(BaseTrigger),true)]
    public class BaseTriggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var script = target as BaseTrigger;

            if (script == null || script.Descriptions == null || script.eventName == null)
            {
                base.OnInspectorGUI();
                return;
            }
            
            if (script.Descriptions != null && script.Descriptions.ContainsKey(script.eventName))
            {
                var description = script.Descriptions[script.eventName];
                EditorGUILayout.HelpBox(description, MessageType.Info);
            }
            
            base.OnInspectorGUI();
        }
    }
}
