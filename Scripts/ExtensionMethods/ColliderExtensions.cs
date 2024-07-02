using UnityEngine;

namespace Helpers
{
    public static class ColliderExtensions
    {
        public static MonoBehaviour GetComponentOnBodyOrSelf(this Collider collider)
        {
            if (collider.attachedRigidbody)
            {
                var component = collider.attachedRigidbody.GetComponent<MonoBehaviour>();
                if (component != null)
                {
                    return component;
                }
            }
            
            var selfComponent = collider.GetComponent<MonoBehaviour>();
            return selfComponent;
        }
        
        public static T GetComponentOnSelfOrParent<T>(this Collider collider) where T : Component
        {
            return collider.gameObject.GetComponentOnObjectOrParent<T>();
        }
    }
}