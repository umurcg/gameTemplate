using CorePublic.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    [CustomEditor(typeof(LevelFunnel))]
    public class LevelFunnelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            
            // Add rename tool section
            EditorGUILayout.LabelField("Level Rename Tool", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Open Level Rename Tool"))
            {
                OpenLevelRenameToolForThisFunnel();
            }
        }
        
        private void OpenLevelRenameToolForThisFunnel()
        {
            LevelFunnel levelFunnel = (LevelFunnel)target;
            
            // Open the rename tool window
            LevelFunnelRenameTool window = EditorWindow.GetWindow<LevelFunnelRenameTool>("Level Funnel Rename Tool");
            
            // Set the selected level funnel to this one
            window.selectedLevelFunnel = levelFunnel;
            
            // Automatically get levels from the funnel
            window.GetLevelsFromLevelFunnel();
            
            // Focus the window
            window.Focus();
        }
    }
} 