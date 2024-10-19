using CorePublic.Classes;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{

    public class OnTrigger : MonoBehaviour
    {
        public UnityEvent<GameObject> triggerEnter;
        public UnityEvent<GameObject> triggerExit;
        [SerializeField]private bool destroyOnEnter;
        [SerializeField]private bool destroyOnExit;
        
        [SerializeField]private Tags tags;
        

 
        

        private void OnTriggerEnter(Collider other)
        {
            if (tags.Contains(other.gameObject))
            {
                triggerEnter?.Invoke(other.gameObject);
                if (destroyOnEnter)
                    Destroy(gameObject);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (tags.Contains(other.gameObject))
            {
                triggerExit?.Invoke(other.gameObject);
                if (destroyOnEnter)
                    Destroy(gameObject);
                
            }
        }

      
    }
}