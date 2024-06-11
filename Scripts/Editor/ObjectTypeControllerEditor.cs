using UnityEditor;
using UnityEngine;

namespace ObjectType.Editor
{
    [CustomEditor(typeof(ObjectTypeController))]
    public class ObjectTypeControllerEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var objectType = (ObjectTypeController) target;
            if (GUILayout.Button("Set Type"))
            {
                objectType.SetObjectType(ObjectTypeLibrary.Find().FindObjectType(objectType.testType.typeName));
                EditorUtility.SetDirty(target);
                
                var listeners=objectType.GetComponentsInChildren<IObjectTypeListener>();
                foreach (var listener in listeners)
                {
                    var listenerObject = (MonoBehaviour) listener;
                    EditorUtility.SetDirty(listenerObject);
                }
                
            }
        }
    }
}