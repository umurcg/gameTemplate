using UnityEngine;

namespace Helpers
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        #region Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static T _instance;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<T>();

                return _instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Use this for initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            // DontDestroyOnLoad ( gameObject );
            else if (_instance != this) Destroy(this);
        }

        public static T Request()
        {
            if (!_instance) Debug.LogWarning("There is no instance of " + typeof(T).Name + " in the scene");

            return _instance;
        }

        public static T ForceRequest()
        {
            if (_instance == null)
            {
                var ownerObject = new GameObject();
                _instance = ownerObject.AddComponent<T>();
                ownerObject.name = typeof(T).ToString();
            }

            return _instance;
        }

        #endregion
    }
}