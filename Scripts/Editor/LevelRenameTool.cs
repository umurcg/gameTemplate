using CorePublic.Managers;
using CorePublic.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    
    public class LevelRenameTool:EditorWindow
    {
        
        public string namePrefix;
        public LevelData[] levels;
        public int startIndex;
        
        [MenuItem("Reboot/Tools/Level Rename Tool")]
        public static void OpenWindow()
        {
            GetWindow<LevelRenameTool>("Level Rename Tool");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Rename Levels", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            
            //Draw gameobject array
            SerializedObject so = new SerializedObject(this);
            SerializedProperty levelsProperty = so.FindProperty("levels");
            EditorGUILayout.PropertyField(levelsProperty, true);
            so.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            namePrefix = EditorGUILayout.TextField("Name Prefix", namePrefix);
            EditorGUILayout.Space();
            
            startIndex = EditorGUILayout.IntField("Start Index", startIndex);
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Get Levels"))
            {
                GetLevelsFromLevelManager();
            }
            
            if (GUILayout.Button("Rename"))
            {
                RenameLevels();
            }
        }

        public void GetLevelsFromLevelManager()
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levels = levelManager.GetLevels();
        }
        
        private void RenameLevels()
        {
            //First rename files to temp names to avoid conflicts
            for (int index = 0; index < levels.Length; index++)
            {
                LevelData level = levels[index];
                //Get path of the asset
                string path = AssetDatabase.GetAssetPath(level);
                //Random name to avoid conflicts
                string newName = "temp" + index;
                AssetDatabase.RenameAsset(path, newName);

                //Save the changes
                AssetDatabase.SaveAssets();
            }
            
            
            for (int index = 0; index < levels.Length; index++)
            {
                LevelData level = levels[index];
                //Get path of the asset
                string path = AssetDatabase.GetAssetPath(level);

                //Rename the level
                string newName = namePrefix + (index + startIndex);
                AssetDatabase.RenameAsset(path, newName);

                //Save the changes
                AssetDatabase.SaveAssets();
            }
        }
    }
}