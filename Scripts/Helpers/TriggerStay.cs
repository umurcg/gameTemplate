using CorePublic.Classes;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class TriggerStay : MonoBehaviour
    {
        public UnityEvent<GameObject> triggerStay;
        [SerializeField] private bool destroyOnTrigger;
        [SerializeField] private Tags tags;

        private void OnTriggerStay(Collider other)
        {
            if (tags.Contains(other))
            {
                triggerStay?.Invoke(other.gameObject);
                if (destroyOnTrigger) Destroy(gameObject);
            }
        }
    }
}