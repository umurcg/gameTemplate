using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class OnCollision : MonoBehaviour
    {
        public UnityEvent<GameObject> collisionEnter;
        public UnityEvent<GameObject> collisionExit;

        public string[] tags;

        private void OnCollisionEnter(Collision other)
        {
            if (tags.Contains(other.gameObject.tag)) collisionEnter?.Invoke(other.gameObject);
        }


        private void OnCollisionExit(Collision other)
        {
            if (tags.Contains(other.gameObject.tag)) collisionExit?.Invoke(other.gameObject);
        }
    }
}