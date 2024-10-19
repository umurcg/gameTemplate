using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CorePublic.UI
{
    public class TextController : MonoBehaviour
    {
        public string prefix = "";
        public string suffix = "";
        private TextMeshPro _textMesh;
        private TextMeshProUGUI _textMeshUI;
        private Text _textUI;

        public bool IsNumber;

        [SerializeField] private bool KMBFormatter;
        [SerializeField] private string numberFormatter = "F0";

        public Action CountCompleted;

        private bool _initialized;

        private void Initialize()
        {
            if (_initialized) return;
            _textUI = GetComponent<Text>();
            _textMesh = GetComponent<TextMeshPro>();
            _textMeshUI = GetComponent<TextMeshProUGUI>();
            _initialized = true;

            if (!_textUI && !_textMesh && !_textMeshUI)
            {
                Debug.LogError("There is no text component attached to this object");
                return;
            }
        }

        public void SetText(float value)
        {
            string text = value.ToString(numberFormatter);
            if (KMBFormatter)
            {
                text = FormatNumber(Mathf.RoundToInt(value));
            }
            SetText(text);
        }

        public void SetText(int value)
        {
            string text = value.ToString(numberFormatter);
            if (KMBFormatter)
            {
                text = FormatNumber(value);
            }
            SetText(text);
        }

        [ContextMenu("Count Up")]
        public void CountUp(int aim, float duration)
        {
            StartCoroutine(CountUpCoroutine(aim, duration));
        }

        private IEnumerator CountUpCoroutine(int aim, float duration)
        {
            float timer = 0;
            SetText(0);
            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float r = timer / duration;
                SetText(Mathf.Lerp(0, aim, r));
                yield return null;
            }

            CountCompleted?.Invoke();
        }

        public void SetText(string text)
        {
            if (!_initialized) Initialize();

            if (_textUI != null)
                _textUI.text = prefix + text + suffix;
            else if (_textMesh != null)
                _textMesh.text = prefix + text + suffix;
            else if (_textMeshUI != null)
                _textMeshUI.text = prefix + text + suffix;
            else
                Debug.LogError("There are no text components on this object!");
        }

        public void UpdateText(string text)
        {
            SetText(text);
        }

        public string FormatNumber(int num)
        {
            if (num >= 1000000000) return (num / 1000000000f).ToString(numberFormatter) + "B";
            if (num >= 1000000) return (num / 1000000f).ToString(numberFormatter) + "M";
            if (num >= 1000) return (num / 1000f).ToString(numberFormatter) + "K";
            return num.ToString("F0");
        }
    }
}
