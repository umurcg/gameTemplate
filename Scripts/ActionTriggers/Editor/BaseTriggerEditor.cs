using System;
using ActionTriggers;
using UnityEditor;


[CustomEditor(typeof(BaseTrigger),true)]
public class BaseTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as BaseTrigger;
        
        //Dropdown with all events
        string[] options = script.GetEvents();
        int index = Array.IndexOf(options, script.eventName);
        var newIndex = EditorGUILayout.Popup("Event", index, options);

        if (newIndex != index)
        {
            script.eventName = options[newIndex];
            EditorUtility.SetDirty(script);
        }

        base.OnInspectorGUI();
    }
}