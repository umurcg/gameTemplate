using System.Collections;
using UnityEngine;

namespace CorePublic.Helpers
{
    [AddComponentMenu("*Reboot/ArtistTools/MaterialFloatLerpTrigger")]
    public class MaterialFloatLerpTrigger : MonoBehaviour
    {
        [SerializeField] private string propertyName;
        public enum Mode
        {
            GlobalMaterial, MeshRenderer, SkinnedMeshRenderer, SpriteRenderer
        }

        public enum ParameterType
        {
            Float, Vector, Color
        }

        public ParameterType parameterType;

        public Mode mode;
        [SerializeField] private Material globalMaterial;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private float aimValue;
        [SerializeField] private Vector3 aimValueVector;
        [SerializeField] private Color aimValueColor;
        [SerializeField] private float duration;

        private float DeltaTime => unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        [SerializeField] private bool unscaledTime;

        public void Trigger()
        {
            if (mode == Mode.SpriteRenderer)
            {
                StartCoroutine(LerpColor(spriteRenderer.material, propertyName, aimValueColor, duration));
                return;
            }

            Material material = null;
            switch (mode)
            {
                case Mode.GlobalMaterial:
                    material = globalMaterial;
                    break;
                case Mode.MeshRenderer:
                    material = meshRenderer.material;
                    break;
                case Mode.SkinnedMeshRenderer:
                    material = skinnedMeshRenderer.material;
                    break;
            }

            if (parameterType == ParameterType.Float)
                StartCoroutine(LerpFloat(material, propertyName, aimValue, duration));
            else if (parameterType == ParameterType.Vector)
                StartCoroutine(LerpVector(material, propertyName, aimValueVector, duration));
            else if (parameterType == ParameterType.Color)
                StartCoroutine(LerpColor(material, propertyName, aimValueColor, duration));
        }

        public void LerpColor(Color color)
        {
            if (mode == Mode.SpriteRenderer)
            {
                StartCoroutine(LerpColor(spriteRenderer.material, propertyName, color, duration));
                return;
            }

            var material = mode == Mode.GlobalMaterial ? globalMaterial : mode == Mode.MeshRenderer ? meshRenderer.material : skinnedMeshRenderer.material;
            StartCoroutine(LerpColor(material, propertyName, color, duration));
        }

        public void LerpVector(Vector3 vector3)
        {
            var material = mode == Mode.GlobalMaterial ? globalMaterial : mode == Mode.MeshRenderer ? meshRenderer.material : skinnedMeshRenderer.material;
            StartCoroutine(LerpVector(material, propertyName, vector3, duration));
        }

        public void LerpFloat(float f)
        {
            var material = mode == Mode.GlobalMaterial ? globalMaterial : mode == Mode.MeshRenderer ? meshRenderer.material : skinnedMeshRenderer.material;
            StartCoroutine(LerpFloat(material, propertyName, f, duration));
        }

        public void Cancel()
        {
            StopAllCoroutines();
        }

        private IEnumerator LerpColor(Material material, string s, Color color, float f)
        {
            var startColor = material.GetColor(s);
            var t = 0f;
            while (t < 1)
            {
                t += DeltaTime / f;
                material.SetColor(s, Color.Lerp(startColor, color, t));
                yield return null;
            }
        }

        private IEnumerator LerpVector(Material material, string s, Vector3 vector3, float f)
        {
            var startVector = material.GetVector(s);
            var t = 0f;
            while (t < 1)
            {
                t += DeltaTime / f;
                material.SetVector(s, Vector3.Lerp(startVector, vector3, t));
                yield return null;
            }
        }

        private IEnumerator LerpFloat(Material material, string s, float f, float duration1)
        {
            float t = 0;
            float startValue = material.GetFloat(s);
            while (t < duration1)
            {
                t += DeltaTime;
                material.SetFloat(s, Mathf.Lerp(startValue, f, t / duration1));
                yield return null;
            }
        }

        // Sprite renderer lerper
        private IEnumerator LerpColor(SpriteRenderer spriteRenderer, string s, Color color, float f)
        {
            var startColor = spriteRenderer.color;
            var t = 0f;
            while (t < 1)
            {
                t += DeltaTime / f;
                spriteRenderer.color = Color.Lerp(startColor, color, t);
                yield return null;
            }
        }
    }
}
