using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Classes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Helpers
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