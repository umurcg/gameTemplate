using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDropper : MonoBehaviour
{
    public int FPSDropThreshold = 40;
    public int DropFPS = 30;
    
    private int frameCount;
    public float monitoringDuration = 10f;
    private float monitoringStartTime;


    private void Start()
    {
        StartCoroutine(MonitorFPS());
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

                if (averageFps < FPSDropThreshold)
                {
                    Application.targetFrameRate = DropFPS;
                    //No need to monitor anymore
                    Destroy(this);
                    yield break;
                }

                frameCount = 0;
                monitoringStartTime = Time.time;
            }

            yield return null;
        }
    }
}
