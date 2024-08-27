using System;
using UnityEngine;

public abstract class SettingsManagerBase<T> : ScriptableObject where T : SettingsManagerBase<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>(typeof(T).Name);
                if (instance == null)
                {
                    Debug.LogError($"Failed to load settings for {typeof(T).Name}. Please ensure you have created a settings asset.");
                }
                else
                {
                    instance.LoadSettings();
                }
            }
            return instance;
        }
    }

    protected abstract string RemoteConfigKey { get; }

    protected virtual void LoadSettings()
    {
        if (RemoteConfig.Instance != null)
        {
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