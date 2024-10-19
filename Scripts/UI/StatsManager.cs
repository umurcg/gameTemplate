using System.Collections;
using System.Linq;
using CorePublic.Interfaces;
using CorePublic.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace CorePublic.UI
{
    public class StatsManager : MonoBehaviour
    {
        private Text _text;
        private IStats[] _allStats;
        private Button debugSwitchButton;

        public IEnumerator Start()
        {
            if (!Debug.isDebugBuild && RemoteConfig.Instance && !RemoteConfig.Instance.GetBool("isStatsEnabled",false))
            {
                Destroy(gameObject);
                yield break;
            }
            
            yield return null;
            
            _text = GetComponent<Text>();
            if (!_text)
            {
                _text = gameObject.AddComponent<Text>();
                _text.alignment = TextAnchor.LowerRight;
                _text.rectTransform.anchorMin = new Vector2(0, 0);
                _text.rectTransform.anchorMax = new Vector2(1, 1);
                _text.rectTransform.offsetMax = new Vector2(-10, -10);
                _text.rectTransform.offsetMin = new Vector2(10, 10);

                _text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                _text.fontSize = 9;
                _text.raycastTarget = false;
            }

            debugSwitchButton = GetComponentInChildren<Button>();
            if (debugSwitchButton == null)
            {
                //Create button
                var buttonRectTransform =  new GameObject("DebugSwitchButton").AddComponent<RectTransform>();
                buttonRectTransform.SetParent(transform);
                buttonRectTransform.transform.localScale = Vector3.one;
                
                var image=buttonRectTransform.gameObject.AddComponent<Image>();
                image.color = new Color(0, 0, 0, 0);
                
                debugSwitchButton = buttonRectTransform.gameObject.AddComponent<Button>();
                debugSwitchButton.transform.SetParent(transform);
                //Set button position to bottom right of screen and set its size to .05 of viewport
                 debugSwitchButton.GetComponent<RectTransform>();
                buttonRectTransform.anchorMin = new Vector2(1, 0);
                buttonRectTransform.anchorMax = new Vector2(1, 0);
                float width = Screen.width * .05f;
                buttonRectTransform.sizeDelta = new Vector2(width, width);
                buttonRectTransform.anchoredPosition = new Vector2(-width/2, width/2);
                
                debugSwitchButton.transition = Selectable.Transition.ColorTint;
                debugSwitchButton.targetGraphic = image;
                
            }
            
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
            if(_allStats==null) return;
            string textString = "";
            foreach (var stat in _allStats)
            {
                var stats = stat.GetStats();
                if(stats!=null)
                    textString += stat.GetStats() + "\n---------------------------------\n";
            }

            _text.text = textString;

        }
    }
}
