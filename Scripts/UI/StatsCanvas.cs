using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private int updatePeriod = 30;
        private static StatsCanvas _instance;
        private static HashSet<IStats> _registeredStats = new HashSet<IStats>();
        private static bool _needsUpdate = false;
        
        private Text _text;
        private Button debugSwitchButton;
        private bool _initialized = false;
        private string _cachedStatsText = "";

        public static StatsCanvas Instance => _instance;
        public float panelWidth = 100;

        /// <summary>
        /// Register a stats object. Call this when your object becomes active or in Start/Awake.
        /// </summary>
        public static void RegisterStats(IStats stats)
        {
            if (stats != null && _registeredStats.Add(stats))
            {
                _needsUpdate = true;
                Debug.Log($"Registered stats: {stats.GetType().Name}");
            }
        }

        /// <summary>
        /// Unregister a stats object. Call this in OnDestroy or when your object becomes inactive.
        /// </summary>
        public static void UnregisterStats(IStats stats)
        {
            if (stats != null && _registeredStats.Remove(stats))
            {
                _needsUpdate = true;
                Debug.Log($"Unregistered stats: {stats.GetType().Name}");
            }
        }

        public IEnumerator Start()
        {
            // Ensure singleton
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                yield break;
            }
            
            _instance = this;
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



            if (!_text)
            {
                var textRectTransform = new GameObject("Text").AddComponent<RectTransform>();
                textRectTransform.SetParent(transform);
                textRectTransform.transform.localScale = Vector3.one;
                textRectTransform.anchorMin = new Vector2(1, 0);
                textRectTransform.anchorMax = new Vector2(1, 0);
                textRectTransform.offsetMax = new Vector2(-panelWidth, -panelWidth);
                textRectTransform.offsetMin = new Vector2(panelWidth, panelWidth);

                textRectTransform.sizeDelta = new Vector2(panelWidth, panelWidth);
                textRectTransform.anchoredPosition = new Vector2(-panelWidth / 2, panelWidth / 2);

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
                
                buttonRectTransform.sizeDelta = new Vector2(panelWidth, panelWidth);
                buttonRectTransform.anchoredPosition = new Vector2(-panelWidth / 2, panelWidth / 2);

                debugSwitchButton.transition = Selectable.Transition.ColorTint;
                debugSwitchButton.targetGraphic = image;
                debugSwitchButton.onClick.AddListener(() =>
                {
                    _text.enabled = !_text.enabled;
                });
            }

            // Clean up any null references that might exist
            CleanupDestroyedStats();
            _needsUpdate = true;
        }

        /// <summary>
        /// Clean up any destroyed objects from the registered stats
        /// </summary>
        private void CleanupDestroyedStats()
        {
            var toRemove = new List<IStats>();
            foreach (var stat in _registeredStats)
            {
                // Check if the object implementing IStats has been destroyed
                if (stat is MonoBehaviour mb && mb == null)
                {
                    toRemove.Add(stat);
                }
            }

            foreach (var stat in toRemove)
            {
                _registeredStats.Remove(stat);
            }

            if (toRemove.Count > 0)
            {
                _needsUpdate = true;
            }
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        public void AddStats(IStats stats)
        {
            RegisterStats(stats);
        }

        /// <summary>
        /// Legacy method for backward compatibility
        /// </summary>
        public void RemoveStats(IStats stats)
        {
            UnregisterStats(stats);
        }

        private void Update()
        {
            if (!_initialized) return;

            // Periodically clean up destroyed objects (every 60 frames to avoid performance hit)
            if (Time.frameCount % 60 == 0)
            {
                CleanupDestroyedStats();
            }

            // Only update text when needed to improve performance
            if (_needsUpdate || Time.frameCount % updatePeriod == 0) // Update every 30 frames for real-time stats
            {
                UpdateStatsText();
                _needsUpdate = false;
            }
        }

        private void UpdateStatsText()
        {
            if (_registeredStats.Count == 0)
            {
                _cachedStatsText = "No stats registered";
                _text.text = _cachedStatsText;
                return;
            }

            string textString = "";
            foreach (var stat in _registeredStats)
            {
                try
                {
                    // Skip if the MonoBehaviour has been destroyed
                    if (stat is MonoBehaviour mb && mb == null)
                        continue;

                    var stats = stat.GetStats();
                    if (!string.IsNullOrEmpty(stats))
                        textString += stats + "\n---------------------------------\n";
                }
                catch (System.Exception e)
                {
                    // Handle cases where GetStats() might throw exceptions
                    textString += $"[Error getting stats from {stat.GetType().Name}]: {e.Message}\n---------------------------------\n";
                }
            }

            _cachedStatsText = textString;
            _text.text = _cachedStatsText;
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
