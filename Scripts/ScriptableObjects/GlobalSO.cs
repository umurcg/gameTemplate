using System;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    public abstract class GlobalSO<T> : ScriptableObject where T : GlobalSO<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GlobalScriptableObjectManager goManager = null;
                    
                    if(Application.isPlaying==false)
                        goManager = FindObjectOfType<GlobalScriptableObjectManager>();
                    else
                        goManager = GlobalScriptableObjectManager.Instance;
                    

                    if(goManager == null)
                    {
                        Debug.LogError($"GlobalScriptableObjectManager not found. Please ensure you have created a GlobalScriptableObjectManager in the scene.");
                        return null;
                    }

                    _instance = goManager.FindGlobalSO<T>();
                    if (_instance == null)
                    {
                        Debug.LogWarning($"Failed to load settings for {typeof(T).Name}. Please ensure you have created a settings asset. To handle the issue, a new instance of the settings will be created.");
                        _instance = CreateInstance();
                    }
                    else
                    {
                        _instance.LoadSettings();
                    }
                }
                return _instance;
            }
        }

        protected static T CreateInstance()
        {
            _instance = CreateInstance<T>();
            return _instance;
        }
        
        /// <summary>
        /// If not null, it will be used to load settings from remote config. If null, it means this settings will not be loaded from remote config.
        /// To enable remote config, fill the RemoteConfigKey at the child class.
        /// </summary>
        protected virtual string RemoteConfigKey { get; } = null;

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

        }
    }
}