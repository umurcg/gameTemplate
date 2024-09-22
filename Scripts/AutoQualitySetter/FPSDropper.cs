using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDropper : MonoBehaviour
{
    
    public Action<int> OnFPSDropped;
    public int fpsDropThreshold = 40;
    public int dropFPS = 30;
    public float monitoringDuration = 5f;
    
    private int _frameCount;
    private float _monitoringStartTime;
    


    private void Start()
    {
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
