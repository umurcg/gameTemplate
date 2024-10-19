using System.Collections;
using CorePublic.Interfaces;
using UnityEngine;

namespace CorePublic.AutoQualitySetter
{
    public class AutoQualityAdjuster : MonoBehaviour, IStats
    {
        public bool enableDynamicQualityAdjustment = true;

        public int fpsThreshold = 30;
        public float monitoringDuration = 10f;

        private int frameCount;
        private float monitoringStartTime;

        public GeekbenchDeviceList benchData;

        public int lowBenchmarkThreshold = 250;
        public int mediumBenchmarkThreshold = 1000;

        private GeekbenchData foundBenchmarkData;
    
        public int SavedQualityLevel
        {
            get
            {
                return PlayerPrefs.GetInt("Quality", -1);
            }
            set
            {
                PlayerPrefs.SetInt("Quality", value);
            }
        }

        void Start()
        {
            if (!Application.isEditor) AdjustQualitySettings();

            if (enableDynamicQualityAdjustment)
            {
                frameCount = 0;
                monitoringStartTime = Time.time;
                StartCoroutine(MonitorFPS());
            }
        }

        private void AdjustQualitySettings()
        {
            if (SavedQualityLevel != -1)
            {
                SetQualityLevel(SavedQualityLevel);
                return;
            }

            int level = -1;
        
            foundBenchmarkData = benchData.GetBenchDataOfDevice();
            if (foundBenchmarkData != null)
            {
                level = SetQualityLevelBasedOnBenchmarkScore(foundBenchmarkData.score);
                string deviceModel = SystemInfo.deviceModel;
                string benchmarkName = foundBenchmarkData.name;
                // Send benchmark success event to GameAnalytics
            }
            else
            {
                level = GetRecommendedQualityLevel();
                SetQualityLevel(level);
                // Send event about unsuccessful device benchmark detection
            }
        
            SavedQualityLevel = level;
        }

        private int SetQualityLevelBasedOnBenchmarkScore(int benchmarkScore)
        {
            int qualityLevel = 0;
        
            if (benchmarkScore <= lowBenchmarkThreshold)
            {
                qualityLevel = 0; // Low
            }
            else if (benchmarkScore > lowBenchmarkThreshold && benchmarkScore <= mediumBenchmarkThreshold)
            {
                qualityLevel = 1; // Medium
            }
            else
            {
                qualityLevel = 2; // High
            }
        
            SetQualityLevel(qualityLevel);
            return qualityLevel;
        }

        private void SetQualityLevel(int qualityLevel)
        {
            QualitySettings.SetQualityLevel(qualityLevel);
        }

        private int GetRecommendedQualityLevel()
        {
            int systemMemorySize = SystemInfo.systemMemorySize;
            int processorCount = SystemInfo.processorCount;
            int graphicsMemorySize = SystemInfo.graphicsMemorySize;

            if (systemMemorySize <= 2048 && processorCount <= 4 && graphicsMemorySize <= 1024)
                return 0; // Low
            else if (((systemMemorySize > 2048 && systemMemorySize <= 4096) ||
                      (processorCount > 4 && processorCount <= 6)) && graphicsMemorySize <= 2048)
                return 1; // Medium
            else if (systemMemorySize > 4096 && processorCount > 6 && graphicsMemorySize > 2048)
                return 2; // High
            else
                return 1; // Medium as fallback
        }

        IEnumerator MonitorFPS()
        {
            while (true)
            {
                frameCount++;
                float elapsedTime = Time.time - monitoringStartTime;

                if (elapsedTime >= monitoringDuration)
                {
                    float averageFps = frameCount / elapsedTime;

                    if (averageFps < fpsThreshold && QualitySettings.GetQualityLevel() > 0)
                    {
                        int newQualityLevel = QualitySettings.GetQualityLevel() - 1;
                        QualitySettings.SetQualityLevel(newQualityLevel);
                    }

                    frameCount = 0;
                    monitoringStartTime = Time.time;
                }

                yield return null;
            }
        }

        public string GetStats()
        {
            string text =
                "Device Model: " + SystemInfo.deviceModel +
                "\nQuality Level: " + (QualitySettings.GetQualityLevel() == 0 ? "Low" :
                    QualitySettings.GetQualityLevel() == 1 ? "Medium" : "High") +
                "\nGraphics Device: " + SystemInfo.graphicsDeviceName +
                "\nGraphics Memory Size: " + SystemInfo.graphicsMemorySize + " MB" +
                "\nOperating System: " + SystemInfo.operatingSystem +
                "\nCPU Type: " + SystemInfo.processorType +
                "\nCPU Count: " + SystemInfo.processorCount;

            if (foundBenchmarkData != null)
            {
                text += "\nBenchmark Score: " + foundBenchmarkData.score;
            }
            else
            {
                text += "\nBenchmark Score: Not Found";
            }

            return text;
        }
    }
}
