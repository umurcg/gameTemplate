using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using CorePublic.Helpers;
using CorePublic.Managers;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Path = System.IO.Path;

public class PlayTestDataCollector : Singleton<PlayTestDataCollector>
{
    [SerializeField] private bool enablePersistence = true;
    private string _playerName;
    private List<LevelPlayData> _levelPlayDatas = new List<LevelPlayData>();
    private float _currentLevelStartTime;
    private const string _playerPrefsKey = "PlayTestData";

    void Start()
    {
        if (enablePersistence)
        {
            LoadData();
        }

        GlobalActions.OnGameStarted += OnGameStarted;
        GlobalActions.OnGameLost += OnGameLost;
        GlobalActions.OnGameWin += OnGameWin;
        GlobalActions.OnGameRevived += OnGameRevived;
    }

    private void OnDestroy()
    {
        if (enablePersistence)
        {
            SaveData();
        }

        GlobalActions.OnGameStarted -= OnGameStarted;
        GlobalActions.OnGameLost -= OnGameLost;
        GlobalActions.OnGameWin -= OnGameWin;
        GlobalActions.OnGameRevived -= OnGameRevived;
    }

    private void OnGameStarted()
    {
        int levelIndex = CoreManager.Instance.Level;
        EnsureLevelDataExists(levelIndex);

        _levelPlayDatas[levelIndex].levelName = LevelManager.Instance.ActiveLevelData?.name ??
                                                LevelManager.Instance.ActiveLevelObject?.name ??
                                                "Unknown";
        _currentLevelStartTime = Time.time;
    }

    private void OnGameWin()
    {
        int levelIndex = CoreManager.Instance.Level;
        EnsureLevelDataExists(levelIndex);
        _levelPlayDatas[levelIndex].isWin = true;
        _levelPlayDatas[levelIndex].winDuration = Time.time - _currentLevelStartTime;
    }

    private void OnGameRevived()
    {
        int levelIndex = CoreManager.Instance.Level;
        EnsureLevelDataExists(levelIndex);
        _levelPlayDatas[levelIndex].reviveCount++;
    }

    private void OnGameLost()
    {
        int levelIndex = CoreManager.Instance.Level;
        EnsureLevelDataExists(levelIndex);
        _levelPlayDatas[levelIndex].levelLoseCount++;
        _levelPlayDatas[levelIndex].loseDurations.Add(Time.time - _currentLevelStartTime);
    }

    private void EnsureLevelDataExists(int levelIndex)
    {
        while (_levelPlayDatas.Count <= levelIndex)
        {
            _levelPlayDatas.Add(new LevelPlayData(levelIndex));
        }
    }

    #if ODIN_INSPECTOR
    [Button]
    #endif
    public string ExportToCsv()
    {
        int maxLoseCount = 0;
        foreach (var levelData in _levelPlayDatas)
        {
            if (levelData.loseDurations.Count > maxLoseCount)
            {
                maxLoseCount = levelData.loseDurations.Count;
            }
        }

        StringBuilder csvContent = new StringBuilder();
        csvContent.Append("LevelIndex,LevelName,LevelLoseCount,WinDuration,ReviveCount,IsWin,TotalDuration");
        for (int i = 1; i <= maxLoseCount; i++)
        {
            csvContent.Append($",LoseDuration{i}");
        }

        csvContent.AppendLine();

        foreach (var levelData in _levelPlayDatas)
        {
            float totalDuration = levelData.winDuration;
            foreach (var loseDuration in levelData.loseDurations)
            {
                totalDuration += loseDuration;
            }

            csvContent.Append($"{levelData.LevelIndex},{levelData.levelName},{levelData.levelLoseCount}," +
                              $"{levelData.winDuration},{levelData.reviveCount},{levelData.isWin},{totalDuration}");
            for (int i = 0; i < maxLoseCount; i++)
            {
                if (i < levelData.loseDurations.Count)
                {
                    csvContent.Append($",{levelData.loseDurations[i]}");
                }
                else
                {
                    csvContent.Append(",");
                }
            }

            csvContent.AppendLine();
        }

        string filePath = Path.Combine("Assets", $"PlayTestData_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");

        try
        {
            File.WriteAllText(filePath, csvContent.ToString());
            Debug.Log($"Data exported successfully to {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to export data to CSV: {e.Message}");
        }

        return filePath;
    }

    private void SaveData()
    {
        string jsonData = JsonConvert.SerializeObject(_levelPlayDatas);
        PlayerPrefs.SetString(_playerPrefsKey, jsonData);
        PlayerPrefs.Save();
        Debug.Log("Play test data saved successfully.");
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(_playerPrefsKey))
        {
            string jsonData = PlayerPrefs.GetString(_playerPrefsKey);
            _levelPlayDatas = JsonConvert.DeserializeObject<List<LevelPlayData>>(jsonData) ?? new List<LevelPlayData>();
            Debug.Log("Play test data loaded successfully.");
        }
    }
    

    #if ODIN_INSPECTOR
    [Button]
    #endif
    public void SendAsMail()
    {
        // Convert the level data list to a JSON string
        string jsonData = JsonConvert.SerializeObject(_levelPlayDatas, Formatting.Indented);

        // Prepare the mailto link with JSON data in the body
        string recipient = "admin@reboot.ist";
        string subject = EscapeURL("Playtest Data Submission");
        string body = EscapeURL("Playtest Data:\n\n" + jsonData);

        string mailto = $"mailto:{recipient}?subject={subject}&body={body}";

        // Open the default email client
        Application.OpenURL(mailto);
    }
    
    private string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}

[Serializable]
public class LevelPlayData
{
    public int LevelIndex { get; private set; }
    public string levelName;
    public int levelLoseCount;
    public float winDuration;
    public int reviveCount;
    public bool isWin;
    public List<float> loseDurations { get; private set; }

    public LevelPlayData(int levelIndex)
    {
        this.LevelIndex = levelIndex+1;
        loseDurations = new List<float>();
    }
}