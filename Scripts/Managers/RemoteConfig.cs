using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Interfaces;
using Helpers;
using ScriptableObjects;
using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;

public class RemoteConfig : Singleton<RemoteConfig>, IStats, IPrerequisite
{
    public struct UserAttributes {
        public bool expansionFlag;
    }

    public struct AppAttributes {
        public int level;
        public int score;
        public string appVersion;
    }
    
    [SerializeField] public bool showDebugLogs;
    [SerializeField] private string environmentID;
    private float _bornTime;
    
    private bool isReady;

    new async Task Awake()
    {
        base.Awake();
        
        if (Utilities.CheckForInternetConnection()) 
        {
            await InitializeRemoteConfigAsync();
        }
        
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

        var rebootSettings = Resources.Load<RebootSettings>("RebootSettings");

        if (rebootSettings)
        {
            if (Debug.isDebugBuild)
            {
                environmentID = rebootSettings.remoteConfigEnvironmentIDDebug;
            }
            else
            {
                environmentID = rebootSettings.remoteConfigEnvironmentID;    
            }
            
            
        }
        
        RemoteConfigService.Instance.SetEnvironmentID(environmentID);
        RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());

        _bornTime = Time.time;
         
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();
        
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    

    private void ApplyRemoteConfig(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin) {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                break;
        }

        isReady = true;
        
    }
    
    public float GetFloat(string key, float defaultValue = -1)
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        return RemoteConfigService.Instance.appConfig.GetFloat(key, defaultValue);
    }
    
    public int GetInt(string key, int defaultValue = -1)
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        return RemoteConfigService.Instance.appConfig.GetInt(key, defaultValue);
    }
    
    public string GetString(string key, string defaultValue = "")
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        return RemoteConfigService.Instance.appConfig.GetString(key, defaultValue);
    }
    
    public bool GetBool(string key, bool defaultValue = false)
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        return RemoteConfigService.Instance.appConfig.GetBool(key, defaultValue);
    }
    
    public Vector3 GetVector3(string key, Vector3 defaultValue)
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        
        var json = RemoteConfigService.Instance.appConfig.GetJson(key, "");
        try
        {
            return JsonUtility.FromJson<Vector3>(json);
        }
        catch
        {
            Debug.LogError("Vector data is not valid: " + key);
        }

        return defaultValue;
    }
    
    
    public bool HasKey(string key)
    {
        return RemoteConfigService.Instance.appConfig.HasKey(key);
    }
    
    public string GetJson(string key, string defaultValue = "")
    {
        if (!HasKey(key))
        {
            Debug.LogWarning("Key not found: " + key);
            return defaultValue;
        }
        return RemoteConfigService.Instance.appConfig.GetJson(key, defaultValue);
    }


    public string GetStats()
    {
        if (!showDebugLogs) return null;
        
        if (!isReady)
        {
            return "<color=#FF0000> Remote config is not ready! </color>";
        }
        
        var stats = "";
        
        var properties = ConfigManager.appConfig.GetKeys();
        foreach (var key in properties)
        {
            stats += key + ": " + ConfigManager.appConfig.config.GetValue(key) + "\n";
        }
        
        return stats;
        
    }

    private void OnApplicationQuit()
    {
        RemoteConfigService.Instance.FetchCompleted -= ApplyRemoteConfig;
    }

    public bool IsMet()
    {
        return isReady;
    }
}

