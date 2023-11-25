
using System;
using System.Collections;
using Core.Interfaces;
using Helpers;
using Managers;
using UnityEngine;

[RequireComponent(typeof(FPSCounter))]
public class AdaptivePerformance : Singleton<AdaptivePerformance>, IStats
{
    public enum PerformanceTiers
    {
        Low, Medium, High
    }
    
    private CoreManager _coreManager;

    public PerformanceTiers ActivePerformanceTier { get; private set; } = PerformanceTiers.High;
    
    [SerializeField] private bool useAdaptivePerformance = true;

    [SerializeField] private int targetFrameRate = 60;
    [SerializeField] private int lowFPS = 30;
    [SerializeField, Range(0, 1)] private float tierDropFPSThreshold = .8f;
    [SerializeField, Range(0, 1)] private float tierUpFPSThreshold = .9f;
    [SerializeField] private float tierChangeDuration = 5f;
    [SerializeField] private bool persistentPerformanceTier = false;
    [SerializeField] private float detectionDelay = 5;
    [SerializeField] private bool enableTierUpScaling = true;
    
    
    private float _timer;
    
    public Action<AdaptivePerformance.PerformanceTiers> OnPerformanceTierChanged;

    private FPSCounter _fpsCounter;
    
    private IEnumerator Start()
    {
        yield return null;
        
        _fpsCounter = GetComponent<FPSCounter>();
        _coreManager = CoreManager.Request();
        Application.targetFrameRate = targetFrameRate;
        if(persistentPerformanceTier) 
            ActivePerformanceTier = (PerformanceTiers)PlayerPrefs.GetInt("performance_tier", (int)PerformanceTiers.High);
    }

    void LateUpdate()
    {
        if(Time.timeSinceLevelLoad < detectionDelay || !useAdaptivePerformance) return;

        float highThreshold = tierUpFPSThreshold * Application.targetFrameRate;
        float lowThreshold = tierDropFPSThreshold * Application.targetFrameRate;
        
        if (_fpsCounter.fps < lowThreshold)
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer > tierChangeDuration)
            {
                DecreaseTier();
            }
        } else if (enableTierUpScaling && _fpsCounter.fps > highThreshold)
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer > tierChangeDuration)
            {
                IncreaseTier();
            }
        }
        else
        {
            _timer = 0;
        } 
    }
    
    private void IncreaseTier()
    {
        if (ActivePerformanceTier == PerformanceTiers.Low)
        {
            SetTier(PerformanceTiers.Medium);
        }
        else if (ActivePerformanceTier == PerformanceTiers.Medium)
        {
            SetTier(PerformanceTiers.High);
        }
    }
    
    private void DecreaseTier()
    {
        if (ActivePerformanceTier == PerformanceTiers.High)
        {
            SetTier(PerformanceTiers.Medium);
        } 
        else if (ActivePerformanceTier == PerformanceTiers.Medium)
        {
            SetTier(PerformanceTiers.Low);
        }
    }

    [ContextMenu("SetTier Low")]
    private void SetTierLow()
    {
        SetTier(PerformanceTiers.Low);
    }

    [ContextMenu("SetTier Medium")]
    private void SetTierMedium()
    {
        SetTier(PerformanceTiers.Medium);
    }

    [ContextMenu("SetTier High")]
    private void SetTierHigh()
    {
        SetTier(PerformanceTiers.High);
    }

    private void SetTier(PerformanceTiers tier)
    {
        ActivePerformanceTier = tier;
        if (tier == PerformanceTiers.Low)
        {
            Application.targetFrameRate = lowFPS;
        }
        else
        {
            Application.targetFrameRate = targetFrameRate;
        }

        UpdateQualitySettings();
        OnPerformanceTierChanged?.Invoke(ActivePerformanceTier);
        _timer = 0;
    }

    private void OnDestroy()
    {
        if(persistentPerformanceTier) PlayerPrefs.SetInt("performance_tier", (int)ActivePerformanceTier);
    }

    private void UpdateQualitySettings()
    {
        QualitySettings.SetQualityLevel((int)ActivePerformanceTier);
    }

    public string GetStats()
    {
        string stats = "Performance Tier: " + ActivePerformanceTier + "\n";
        stats += "Locked FPS: " + Application.targetFrameRate;  
        return stats;
    }
}
