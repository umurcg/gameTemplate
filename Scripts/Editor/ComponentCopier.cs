using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class ComponentCopier : Editor 
{
    [MenuItem("Tools/Copy Components")]
    private static void CopyComponents() 
    {
        if (Selection.gameObjects.Length != 2) 
        {
            Debug.LogError("You need to select exactly two game objects");
            return;
        }

        GameObject source = Selection.gameObjects[0];
        GameObject target = Selection.gameObjects[1];

        Component[] components = source.GetComponents<Component>();

        bool isAnyComponentCopied = false;

        foreach (Component component in components) 
        {
            if (target.GetComponent(component.GetType()) == null) 
            {
                ComponentUtility.CopyComponent(component);
                Component newComponent = target.AddComponent(component.GetType());
                ComponentUtility.PasteComponentValues(newComponent);
                isAnyComponentCopied = true;

                // Set the dirty flag for the new component so it gets saved.
                EditorUtility.SetDirty(newComponent);
            }
        }

        // If no component has been copied, log that
        if (!isAnyComponentCopied)
        {
            Debug.Log("No new components were copied to the target object because it already contains all components.");
        }

        // Set the dirty flag for the target object so it gets saved.
        EditorUtility.SetDirty(target);
    }

    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    [MenuItem("Tools/Copy Components", true)]
    private static bool CopyComponentsValidation() 
    {
        // Return false if no game object is selected.
        // Also return false if more than 2 game objects are selected.
        return Selection.gameObjects.Length == 2;
    }
}