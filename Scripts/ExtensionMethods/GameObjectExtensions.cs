using UnityEngine;

namespace Helpers
{
    public static class GameObjectExtensions
    {
        public static MonoBehaviour GetComponentOnObjectOrParent<T>(this UnityEngine.GameObject gameObject) where T : UnityEngine.MonoBehaviour
        {
            MonoBehaviour component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInParent<T>();
            }
            
            return component;
        }

        public static MonoBehaviour GetOrAddComponent<T>(this UnityEngine.GameObject gameObject)
            where T : UnityEngine.MonoBehaviour
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}