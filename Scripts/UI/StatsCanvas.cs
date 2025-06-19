using System;
using System.Collections;
using System.Linq;
using CorePublic.Interfaces;
using CorePublic.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CorePublic.UI
{
    [RequireComponent(typeof(Canvas))]
    public class StatsCanvas : MonoBehaviour
    {
        private Text _text;
        private Button debugSwitchButton;
        private IStats[] _allStats;
        private bool _initialized = false;


        public IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);

            if (!Application.isEditor)
            {
                gameObject.SetActive(false);                
                yield break;
            }

            yield return null;
            Initialize();
        }

        public void ActivateStats(){
            gameObject.SetActive(true);
            if(!_initialized){
                Initialize();
            }
        }

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            float width = Screen.width * .2f;

            if (!_text)
            {
                var textRectTransform = new GameObject("Text").AddComponent<RectTransform>();
                textRectTransform.SetParent(transform);
                textRectTransform.transform.localScale = Vector3.one;
                textRectTransform.anchorMin = new Vector2(1, 0);
                textRectTransform.anchorMax = new Vector2(1, 0);
                textRectTransform.offsetMax = new Vector2(-width, -width);
                textRectTransform.offsetMin = new Vector2(width, width);

                textRectTransform.sizeDelta = new Vector2(width, width);
                textRectTransform.anchoredPosition = new Vector2(-width / 2, width / 2);

                _text = textRectTransform.gameObject.AddComponent<Text>();
                _text.alignment = TextAnchor.LowerRight;
      
                _text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                _text.fontSize = 20;
                _text.raycastTarget = false;
            }

            
            if (debugSwitchButton == null)
            {
                //Create button
                var buttonRectTransform = new GameObject("DebugSwitchButton").AddComponent<RectTransform>();
                buttonRectTransform.SetParent(transform);
                buttonRectTransform.transform.localScale = Vector3.one;

                var image = buttonRectTransform.gameObject.AddComponent<Image>();
                image.color = new Color(0, 0, 0, 0);

                debugSwitchButton = buttonRectTransform.gameObject.AddComponent<Button>();
                debugSwitchButton.transform.SetParent(transform);
                //Set button position to bottom right of screen and set its size to .05 of viewport
                debugSwitchButton.GetComponent<RectTransform>();
                buttonRectTransform.anchorMin = new Vector2(1, 0);
                buttonRectTransform.anchorMax = new Vector2(1, 0);
                
                buttonRectTransform.sizeDelta = new Vector2(width, width);
                buttonRectTransform.anchoredPosition = new Vector2(-width / 2, width / 2);

                debugSwitchButton.transition = Selectable.Transition.ColorTint;
                debugSwitchButton.targetGraphic = image;
                debugSwitchButton.onClick.AddListener(() =>
                {
                    _text.enabled = !_text.enabled;
                });

            }

            _allStats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().ToArray();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            //Look for new stats
            _allStats = FindObjectsOfType<MonoBehaviour>().OfType<IStats>().ToArray();
        }

        public void AddStats(IStats stats)
        {
            var list = _allStats.ToList();
            list.Add(stats);
            _allStats = list.ToArray();
        }

        public void RemoveStats(IStats stats)
        {
            var list = _allStats.ToList();
            list.Remove(stats);
            _allStats = list.ToArray();
        }

        private void Update()
        {
            if (_allStats == null) return;
            string textString = "";
            foreach (var stat in _allStats)
            {
                var stats = stat.GetStats();
                if (stats != null)
                    textString += stat.GetStats() + "\n---------------------------------\n";
            }

            _text.text = textString;

        }
    }
}
