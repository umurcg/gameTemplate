using System;
using Managers;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class ResourceBase<T> : ScriptableObject where T : ResourceBase<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);
                    if (_instance == null)
                    {
                        Debug.LogError($"Failed to load settings for {typeof(T).Name}. Please ensure you have created a settings asset.");
                    }
                    else
                    {
                        _instance.LoadSettings();
                    }
                }
                return _instance;
            }
        }

        protected abstract string RemoteConfigKey { get; }

        protected virtual void LoadSettings()
        {
            if(string.IsNullOrEmpty(RemoteConfigKey)) return;
        
            if (RemoteConfig.Instance != null)
            {
                if(RemoteConfig.Instance.HasKey(RemoteConfigKey)==false) return;
                var json = RemoteConfig.Instance.GetJson(RemoteConfigKey);
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        JsonUtility.FromJsonOverwrite(json, this);
                        return;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error parsing remote config for {GetType().Name}: {e.Message}");
                    }
                }
            }

            // If remote config failed or wasn't available, we're already using the ScriptableObject 
            // loaded from Resources, so no need to do anything else here.
        }
    }
}