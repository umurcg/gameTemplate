using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class AdaptivePerformanceEvent : MonoBehaviour
    {
        public UnityEvent highPerformance;
        public UnityEvent mediumPerformance;
        public UnityEvent lowPerformance;
    
        void Start()
        {
            var adaptivePerformance = FindObjectOfType<AdaptivePerformance>();
            if (adaptivePerformance == null)
            {
                Debug.LogError("AdaptivePerformanceEvent: AdaptivePerformance not found");
                return;
            }
            adaptivePerformance.OnPerformanceTierChanged += OnPerformanceTierChanged;
            
        }

        private void OnPerformanceTierChanged(AdaptivePerformance.PerformanceTiers tier)
        {
            switch (tier)
            {
                case AdaptivePerformance.PerformanceTiers.High:
                    highPerformance.Invoke();
                    break;
                case AdaptivePerformance.PerformanceTiers.Medium:
                    mediumPerformance.Invoke();
                    break;
                case AdaptivePerformance.PerformanceTiers.Low:
                    lowPerformance.Invoke();
                    break;
            }
        }
    }
}
