using CorePublic.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    public class LevelFunnelRenameTool : EditorWindow
    {
        public string namePrefix;
        public LevelData[] levels;
        public int startIndex;
        public LevelFunnel selectedLevelFunnel;

        // Add a Vector2 to track the scroll position
        private Vector2 scrollPos;
        
        [MenuItem("Reboot/Tools/Level Funnel Rename Tool")]
        public static void OpenWindow()
        {
            GetWindow<LevelFunnelRenameTool>("Level Funnel Rename Tool");
        }
        
        private void OnGUI()
        {
            // Begin the scroll view block
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true));

            EditorGUILayout.LabelField("Rename Levels in Level Funnel", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Level Funnel selection
            selectedLevelFunnel = EditorGUILayout.ObjectField("Level Funnel", selectedLevelFunnel, typeof(LevelFunnel), false) as LevelFunnel;
            EditorGUILayout.Space();

            // Draw gameobject array
            SerializedObject so = new SerializedObject(this);
            SerializedProperty levelsProperty = so.FindProperty("levels");
            EditorGUILayout.PropertyField(levelsProperty, true);
            so.ApplyModifiedProperties();

            EditorGUILayout.Space();
            namePrefix = EditorGUILayout.TextField("Name Prefix", namePrefix);
            EditorGUILayout.Space();

            startIndex = EditorGUILayout.IntField("Start Index", startIndex);
            EditorGUILayout.Space();

            GUI.enabled = selectedLevelFunnel != null;
            if (GUILayout.Button("Get Levels from Funnel"))
            {
                GetLevelsFromLevelFunnel();
            }
            GUI.enabled = true;
            
            GUI.enabled = levels != null && levels.Length > 0;
            if (GUILayout.Button("Rename"))
            {
                RenameLevels();
            }
            GUI.enabled = true;

            // End the scroll view block
            EditorGUILayout.EndScrollView();
        }

        public void GetLevelsFromLevelFunnel()
        {
            if (selectedLevelFunnel != null)
            {
                levels = selectedLevelFunnel.Levels;
                Debug.Log($"Retrieved {levels.Length} levels from {selectedLevelFunnel.name}");
            }
            else
            {
                Debug.LogWarning("No Level Funnel selected!");
            }
        }
        
        private void RenameLevels()
        {
            if (levels == null || levels.Length == 0)
            {
                Debug.LogWarning("No levels to rename!");
                return;
            }

            if (string.IsNullOrEmpty(namePrefix))
            {
                Debug.LogWarning("Name prefix cannot be empty!");
                return;
            }

            // First rename files to temp names to avoid conflicts
            for (int index = 0; index < levels.Length; index++)
            {
                LevelData level = levels[index];
                if (level == null) continue;
                
                // Get path of the asset
                string path = AssetDatabase.GetAssetPath(level);
                if (string.IsNullOrEmpty(path)) continue;
                
                // Random name to avoid conflicts
                string newName = "temp" + index + "_" + System.Guid.NewGuid().ToString("N")[..8];
                AssetDatabase.RenameAsset(path, newName);
            }

            // Save the changes after temp renaming
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Then rename files to their final names
            for (int index = 0; index < levels.Length; index++)
            {
                LevelData level = levels[index];
                if (level == null) continue;
                
                // Get path of the asset
                string path = AssetDatabase.GetAssetPath(level);
                if (string.IsNullOrEmpty(path)) continue;

                // Rename the level
                string newName = namePrefix + " " + (index + startIndex);
                AssetDatabase.RenameAsset(path, newName);
            }

            // Save the changes after final renaming
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"Successfully renamed {levels.Length} levels with prefix '{namePrefix}' starting from index {startIndex}");
        }
    }
} 