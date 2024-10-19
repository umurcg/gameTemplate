using System.Reflection;
using CorePublic.Helpers;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    [CustomEditor(typeof(ActionBinder))]
    public class ActionBinderEditor : UnityEditor.Editor
    {
        private SerializedProperty _unityEvents;
        private SerializedProperty _unityEventsFloat;
        private SerializedProperty _unityEventsString;
        private SerializedProperty _unityEventsInt;

        private void OnEnable()
        {
            _unityEvents = serializedObject.FindProperty("_unityEvents");
            _unityEventsFloat = serializedObject.FindProperty("_unityEventsFloat");
            _unityEventsString = serializedObject.FindProperty("_unityEventsString");
            _unityEventsInt = serializedObject.FindProperty("_unityEventsInt");

            BindActionsToEvents();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_unityEvents, true);
            EditorGUILayout.PropertyField(_unityEventsFloat, true);
            EditorGUILayout.PropertyField(_unityEventsString, true);
            EditorGUILayout.PropertyField(_unityEventsInt, true);

            serializedObject.ApplyModifiedProperties();
        }

        private void BindActionsToEvents()
        {
            var actionBinder = (ActionBinder)target;
            var components = actionBinder.GetComponents<MonoBehaviour>();

            foreach (var component in components)
            {
                var type = component.GetType();
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 0 && method.ReturnType == typeof(void))
                    {
                        if (!_unityEvents.arraySize.Equals(method.Name))
                        {
                            _unityEvents.InsertArrayElementAtIndex(_unityEvents.arraySize);
                            _unityEvents.GetArrayElementAtIndex(_unityEvents.arraySize - 1).stringValue = method.Name;
                        }
                    }
                    else if (parameters.Length == 1 && method.ReturnType == typeof(void))
                    {
                        var parameterType = parameters[0].ParameterType;

                        if (parameterType == typeof(float))
                        {
                            if (!_unityEventsFloat.arraySize.Equals(method.Name))
                            {
                                _unityEventsFloat.InsertArrayElementAtIndex(_unityEventsFloat.arraySize);
                                _unityEventsFloat.GetArrayElementAtIndex(_unityEventsFloat.arraySize - 1).stringValue = method.Name;
                            }
                        }
                        else if (parameterType == typeof(string))
                        {
                            if (!_unityEventsString.arraySize.Equals(method.Name))
                            {
                                _unityEventsString.InsertArrayElementAtIndex(_unityEventsString.arraySize);
                                _unityEventsString.GetArrayElementAtIndex(_unityEventsString.arraySize - 1).stringValue = method.Name;
                            }
                        }
                        else if (parameterType == typeof(int))
                        {
                            if (!_unityEventsInt.arraySize.Equals(method.Name))
                            {
                                _unityEventsInt.InsertArrayElementAtIndex(_unityEventsInt.arraySize);
                                _unityEventsInt.GetArrayElementAtIndex(_unityEventsInt.arraySize - 1).stringValue = method.Name;
                            }
                        }
                    }
                }
            }
        }
    }
}
