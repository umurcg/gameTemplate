using System;
using System.Collections;
using UnityEngine;

namespace AutoQualitySetter
{
    public class FPSDropper : MonoBehaviour
    {
    
        public Action<int> OnFPSDropped;
        public int fpsDropThreshold = 40;
        public int dropFPS = 30;
        public float monitoringDuration = 5f;
        public bool saveSettings = true;
        private int _frameCount;
        private float _monitoringStartTime;

        private bool IsDropped
        {
            get=> saveSettings && PlayerPrefs.GetInt("FpsIsDropped",0)==1;
            set
            {
                if(saveSettings) PlayerPrefs.SetInt("FpsIsDropped", value ? 1 : 0);
            }
        }


        private void Start()
        {
            if (IsDropped)
            {
                Application.targetFrameRate = dropFPS;
                return;
            }
            StartCoroutine(MonitorFPS());
        }

        IEnumerator MonitorFPS()
        {
            while (true)
            {
                _frameCount++;
                float elapsedTime = Time.time - _monitoringStartTime;

                if (elapsedTime >= monitoringDuration)
                {
                    float averageFps = _frameCount / elapsedTime;

                    if (averageFps < fpsDropThreshold)
                    {
                        Application.targetFrameRate = dropFPS;
                        OnFPSDropped?.Invoke(dropFPS);
                        IsDropped = true;
                        //No need to monitor anymore
                        Destroy(this);
                        yield break;
                    }

                    _frameCount = 0;
                    _monitoringStartTime = Time.time;
                }

                yield return null;
            }
        }
    }
}
