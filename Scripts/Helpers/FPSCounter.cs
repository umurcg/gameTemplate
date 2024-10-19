using UnityEngine;

namespace CorePublic.Helpers
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