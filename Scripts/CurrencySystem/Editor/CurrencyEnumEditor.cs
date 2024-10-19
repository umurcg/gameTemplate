using System;
using UnityEditor;
using UnityEngine;

namespace CorePublic.CurrencySystem.Editor
{
    [CustomPropertyDrawer(typeof(CurrencyEnum))]
    public class CurrencyEnumEditor : PropertyDrawer
    {
        // Draw the dropdown
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (CurrencyData.Instance == null)
            {
                Debug.LogError("CurrencyData is not found in the project. Please create a CurrencyData asset.");
                return;
            }

            var typeNames = CurrencyData.Instance.GetCurrencyNames();
            if (typeNames.Length == 0)
            {
                typeNames = new[] {"No Type"};
            }
            
            EditorGUI.BeginProperty(position, label, property);

            // Find the typeName property
            SerializedProperty currencyProperty = property.FindPropertyRelative("value");

            // Get the current type name and find its index in the list 
            string currentTypeName = currencyProperty.stringValue;
            int currentIndex = Array.IndexOf(typeNames, currentTypeName);
            if (currentIndex == -1) currentIndex = 0; // Default to first item if not found

            // Draw the dropdown
            currentIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            currencyProperty.stringValue = typeNames[currentIndex];

            EditorGUI.EndProperty();
        }
    }
}