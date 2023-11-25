using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RebootConsole : Singleton<RebootConsole>
    {
        private Text _text;
    
        public float logDuration = 1f;
        private List<string> _logMessages=new List<string>();
        private List<float> _logTimes=new List<float>();
    
        public IEnumerator Start()
        {
            if (!RemoteConfig.Instance.GetBool("isStatsEnabled",true))
            {
                Destroy(gameObject);
                yield break;
            }
            
            yield return null;
            
            _text = GetComponent<Text>();
            if (!_text)
            {
                _text = gameObject.AddComponent<Text>();
                _text.alignment = TextAnchor.LowerLeft;
                _text.rectTransform.anchorMin = new Vector2(0, 0);
                _text.rectTransform.anchorMax = new Vector2(1, 1);
                _text.rectTransform.offsetMax = new Vector2(-10, -10);
                _text.rectTransform.offsetMin = new Vector2(10, 10);
                
                _text.font =  Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                _text.fontSize = 9;
                _text.raycastTarget = false;
            }
        }

        private void Update()
        {
            if (_logTimes.Count > 0)
            {
                if (Time.time - _logTimes[0] > logDuration)
                {
                    _logTimes.RemoveAt(0);
                    _logMessages.RemoveAt(0);
                    UpdateText();
                }
            }
        }

        public void UpdateText()
        {
            string text = "";
            foreach (string logMessage in _logMessages)
            {
                text += logMessage + "\n";
            }
            _text.text = text;
        }
    
        public static void Log(string message)
        {
            if (Instance)
            {
                Instance._logMessages.Add(message);
                Instance._logTimes.Add(Time.time);
                Instance.UpdateText();
            }
        }
    
    
    }
}