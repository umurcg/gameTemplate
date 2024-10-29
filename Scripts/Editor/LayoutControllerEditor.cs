using CorePublic.Helpers;
using CorePublic.LevelDesignComponents;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    [CustomEditor(typeof(LayoutController), true)]
    public class LayoutControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LayoutController layout = (LayoutController)target;
        
            if (GUILayout.Button("Update Children Position"))
            {
                layout.UpdateChildrenPosition();
            }
        }
    }
}