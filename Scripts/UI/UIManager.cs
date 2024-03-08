
using Helpers;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [HideInInspector] public Canvas mainCanvas;

        private void Start()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var uiElement = transform.GetChild(i).GetComponent<UIElement>();
                if (uiElement) uiElement.Initialize();
            }

            mainCanvas = GetComponent<Canvas>();
        }

#if UNITY_EDITOR
        [ContextMenu("Prepare For Build")]
        public void PrepareForBuild()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var uiElement = transform.GetChild(i).GetComponent<UIElement>();
                if (uiElement) uiElement.PrepareForBuild();
            }

            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
