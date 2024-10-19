using UnityEngine;

namespace CorePublic.ExtensionMethods
{
    public static class GameObjectExtensions
    {
        public static T GetComponentOnObjectOrParent<T>(this UnityEngine.GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInParent<T>();
            }

            return component;
        }

        public static T GetOrAddComponent<T>(this UnityEngine.GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                gameObject.AddComponent<T>();
            }

            return component;
        }


        public static T GetCachedComponent<T>(this GameObject gameObject) where T : Component
        {
            return ComponentCache.GetComponentCached<T>(gameObject);
        }

        public static T GetCachedComponentInChildren<T>(this GameObject gameObject, bool includeInactive = false)
            where T : Component
        {
            return ComponentCache.GetComponentCachedInChildren<T>(gameObject, includeInactive);
        }
    }
}