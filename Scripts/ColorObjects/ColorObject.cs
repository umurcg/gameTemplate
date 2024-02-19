using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorObjects
{
    public class ColorObject : MonoBehaviour
    {
        public ColorObjectType colorType;
        [SerializeField] private MeshRenderer[] meshRenderers;
        [SerializeField] private SpriteRenderer[] spriteRenderers;
        public ColorDirectory ColorDirectory;

        private void Reset()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Update Visual")]
        public void UpdateVisual()
        {
#if UNITY_EDITOR
            var material = ColorDirectory.FindMaterial(colorType);

            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = material;
                UnityEditor.EditorUtility.SetDirty(meshRenderer);
            }

            var color=ColorDirectory.FindColor(colorType);
            
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = color;
                UnityEditor.EditorUtility.SetDirty(spriteRenderer);
            }
#endif
        }
        
        [ContextMenu("Randomize")]
        public void Randomize()
        {
            if (!ColorDirectory)
            {
                Debug.LogError("ColorDirectory not set");
                return;
            }
            
            colorType = ColorDirectory.GetRandomType();
            UpdateVisual();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu("Set Sibling Name")]
        public void SetSiblingName(string prefix)
        {
            name = $"{prefix}_{transform.GetSiblingIndex()}";
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}


