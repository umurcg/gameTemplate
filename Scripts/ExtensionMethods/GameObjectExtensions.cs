using UnityEngine;

namespace Helpers
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
    }
}