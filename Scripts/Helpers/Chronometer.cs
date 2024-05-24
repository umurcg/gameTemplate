using System;
using Core.Interfaces;
using Managers;
using UnityEngine;

namespace Helpers
{
    public class Chronometer : MonoBehaviour, IStats
    {
        
        [SerializeField] private bool runOnGameplay = true;
        [SerializeField] private float timer;

        private bool _isActive;

        private void Start()
        {
            if (runOnGameplay)
            {
                
                GlobalActions.OnGameStarted += StartChronometer;
                GlobalActions.OnGameWin += PauseChronometer;
                GlobalActions.OnGameLost += PauseChronometer;
                GlobalActions.OnNewLevelLoaded += StopChronometer;
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (_isActive) timer += Time.deltaTime;
        }

        [ContextMenu("Start Chronometer")]
        public void StartChronometer()
        {
            _isActive = true;
        }

        [ContextMenu("Pause Chronometer")]
        public void PauseChronometer()
        {
            _isActive = false;
        }

        [ContextMenu("Stop Chronometer")]
        public void StopChronometer()
        {
            timer = 0;
            _isActive = false;
        }

        public string GetStats()
        {
            var timeSpan = TimeSpan.FromSeconds(timer);
            var timeText =
                $"  {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
            return timeText;
        }
    }
}


// I've removed the Odin Inspector attributes and replaced them with standard Unity features. `[ReadOnly]` was simply removed as Unity doesn't have a readOnly attribute in SerializedField. Odin's `[Button]` attributes have been replaced with Unity's `[ContextMenu]` attribute, which provides similar functionality in the Unity Editor. The context menu items won't show up as buttons in the inspector, but you can still access these functions by right-clicking the component in the Unity Editor.