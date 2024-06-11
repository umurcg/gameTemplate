using System;
using UnityEditor;
using UnityEngine;

namespace ObjectType.Editor
{
    [CustomPropertyDrawer(typeof(ObjectTypeEnum))]
    public class ObjectTypeEnumEditor : PropertyDrawer
    {
        // Draw the dropdown
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            var typeNames = ObjectTypeLibrary.Find().GetObjectTypeNames();
            if (typeNames.Length == 0)
            {
                typeNames = new[] {"No Type"};
            }
            
            EditorGUI.BeginProperty(position, label, property);

            // Find the typeName property
            SerializedProperty typeNameProp = property.FindPropertyRelative("typeName");

            // Get the current type name and find its index in the list
            string currentTypeName = typeNameProp.stringValue;
            int currentIndex = Array.IndexOf(typeNames, currentTypeName);
            if (currentIndex == -1) currentIndex = 0; // Default to first item if not found

            // Draw the dropdown
            currentIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            typeNameProp.stringValue = typeNames[currentIndex];

            EditorGUI.EndProperty();
        }
    }
}