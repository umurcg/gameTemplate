using Core.Interfaces;
using UnityEngine;

namespace Helpers
{
    public class DurationPrerequisite: MonoBehaviour, IPrerequisite
    {
        public float duration;
        private float _startTime;
        
        private void Start()
        {
            _startTime = Time.time;
        }
        
        public bool IsMet()
        {
            return Time.time - _startTime >= duration;
        }
        
    }
}