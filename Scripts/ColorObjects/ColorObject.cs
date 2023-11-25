using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorObjects
{
    public class ColorObject : MonoBehaviour
    {
        public Colors colorType;
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

            var color = GetColor();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = color;
                UnityEditor.EditorUtility.SetDirty(spriteRenderer);
            }
#endif
        }

        private Color GetColor()
        {
            switch (colorType)
            {
                case Colors.Red:
                    return Color.red;
                case Colors.Green:
                    return Color.green;
                case Colors.Blue:
                    return Color.blue;
                case Colors.Yellow:
                    return Color.yellow;
                case Colors.Purple:
                    return Color.magenta;
                case Colors.Orange:
                    return new Color(1, 0.5f, 0);
                case Colors.White:
                    return Color.white;
                case Colors.Black:
                    return Color.black;
                case Colors.Brown:
                    return new Color(0.5f, 0.25f, 0);
                case Colors.Pink:
                    return new Color(1, 0.5f, 0.5f);
                case Colors.Gray:
                    return Color.gray;
                case Colors.Cyan:
                    return Color.cyan;
                case Colors.Magenta:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }


        public bool useAllColors = true;
        [HideInInspector] public List<Colors> colors;

        [ContextMenu("Randomize")]
        public void Randomize()
        {
            if (useAllColors)
            {
                colorType = (Colors)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Colors)).Length);
            }
            else
            {
                colorType = colors[UnityEngine.Random.Range(0, colors.Count)];
            }

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


