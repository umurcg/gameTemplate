using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupController: MonoBehaviour
    {
        public void Activate()
        {
            SetEnable(true);
        }
        
        public void Deactivate()
        {
            SetEnable(false);
        }
        
        public void SetEnable(bool enable)
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = enable ? 1 : 0;
            canvasGroup.interactable = enable;
            canvasGroup.blocksRaycasts = enable;
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(CanvasGroupController))]
    public class CanvasGroupControllerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var script = serializedObject.FindProperty("m_Script");
            root.Add(new PropertyField(script));
            
            
            // Add a button to activate the canvas group
            var activateButton = new Button(() =>
            {
                var canvasGroupController = (CanvasGroupController) target;
                canvasGroupController.Activate();
                EditorUtility.SetDirty(target);
            });
            
            activateButton.text = "Activate";
            
            
            // Add a button to deactivate the canvas group
            var deactivateButton = new Button(() =>
            {
                var canvasGroupController = (CanvasGroupController) target;
                canvasGroupController.Deactivate();
                EditorUtility.SetDirty(target);
            });
            
            deactivateButton.text = "Deactivate";
            
            root.Add(activateButton);
            root.Add(deactivateButton);
            
            return root;
        }
    }
    #endif
}