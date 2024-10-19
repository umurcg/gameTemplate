using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CorePublic.Editor
{
    public class ObjectReplacer : EditorWindow
    {
        public GameObject[] prefabsToReplace;
        public GameObject[] prefabsToReplaceWith;
    
        [MenuItem("Reboot/Tools/Object Replacer")]
        public static void OpenWindow()
        {
            GetWindow<ObjectReplacer>("Object Replacer");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Replace Objects", EditorStyles.boldLabel);
            EditorGUILayout.Space();
        
            //Draw gameobject array
            SerializedObject so = new SerializedObject(this);
            SerializedProperty prefabsToReplaceProperty = so.FindProperty("prefabsToReplace");
            SerializedProperty prefabsToReplaceWithProperty = so.FindProperty("prefabsToReplaceWith");
            EditorGUILayout.PropertyField(prefabsToReplaceProperty, true);
            EditorGUILayout.PropertyField(prefabsToReplaceWithProperty, true);
            so.ApplyModifiedProperties();
        
            EditorGUILayout.Space();
            if (GUILayout.Button("Replace"))
            {
                ReplaceObjects();
            }
        }

        private void ReplaceObjects()
        {
            foreach (GameObject prefab in prefabsToReplace)
            {
                GameObject prefabToReplaceWith = prefabsToReplaceWith[Random.Range(0, prefabsToReplaceWith.Length)];
                if (prefabToReplaceWith != null)
                {
                    var prefabParent = prefab.transform.parent;
                    var prefabPosition = prefab.transform.position;
                    var prefabRotation = prefab.transform.rotation;
                    var prefabScale = prefab.transform.localScale;
                    var prefabTag = prefab.tag;
                
                    DestroyImmediate(prefab);
                    var spawned = PrefabUtility.InstantiatePrefab(prefabToReplaceWith, prefabParent) as GameObject;
                    spawned.transform.position = prefabPosition;
                    spawned.transform.rotation = prefabRotation;
                    spawned.transform.localScale = prefabScale;
                    spawned.tag = prefabTag;
                
                    EditorUtility.SetDirty(spawned);
                
                }        
            }
        }
    }
}
