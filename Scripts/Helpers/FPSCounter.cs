using System;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helpers
{
    public class FPSCounter: MonoBehaviour
    {
        public float fps;
        public float fpsDeltaTime;

        private void Update()
        {
            fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
            fps = 1.0f / fpsDeltaTime;
        }

      
    }
    
    
}