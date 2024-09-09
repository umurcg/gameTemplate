using System.Collections;
using System.Linq;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsManager : MonoBehaviour
    {
        private Text _text;
        private IStats[] _allStats;

        public IEnumerator Start()
        {
            // if (!Debug.isDebugBuild && RemoteConfig.Instance && !RemoteConfig.Instance.GetBool("isStatsEnabled",false))
            // {
            //     Destroy(gameObject);
            //     yield break;
            // }
            //
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
