using System;
using UnityEngine;

namespace Helpers
{
    public class MultiCommandPerFrame : MonoBehaviour
    {
        public delegate void MultiCommand(int commandIndex);
    
        /// <summary>
        /// Creates a scripts that invoke a command more than one within each frame. This script can prevent call misses due to frame rate
        /// </summary>
        /// <param name="command">Command to invoke</param>
        /// <param name="numberOfCommand">Total number of command to call</param>
        /// <param name="maxTotalDuration">Maximum total duration of triggering all commands if command count enough for maximum period.</param>
        /// <param name="maximumPeriod">Maximum period available between each command.With this parameter, one can control maximum duration between each command to prevent slow operations.</param>
        /// <returns></returns>
        public static MultiCommandPerFrame Create(MultiCommand command, int numberOfCommand,float maxTotalDuration, float maximumPeriod)
        {
            GameObject instance = new GameObject("multi_command");
            var multiCommander= instance.AddComponent<MultiCommandPerFrame>();

            multiCommander._totalCommandCount = numberOfCommand;
            multiCommander._command = command;
            multiCommander._commandPeriod = maxTotalDuration / numberOfCommand;

            if (multiCommander._commandPeriod > maximumPeriod)
                multiCommander._commandPeriod = maximumPeriod;

            return multiCommander;

        }
    
    
        /// <summary>
        /// Duration between each command
        /// </summary>
        private float _commandPeriod;

    
        private int _totalCommandCount;
        private MultiCommand _command;
        private int _commandCounter;
        private float remainderTimer;


        public Action OnCommandsCompleted;

        private float bornTime;

        private void Start()
        {
            bornTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;

            remainderTimer += deltaTime;
        
            int numberOfCommandToTrigger= Mathf.FloorToInt(remainderTimer / _commandPeriod);
            if(numberOfCommandToTrigger==0) return;

            for (int i = 0; i < numberOfCommandToTrigger; i++)
            {
                if (_commandCounter < _totalCommandCount)
                {
                    _command.Invoke(_commandCounter);
                    _commandCounter++;
                }
            }
        
            remainderTimer -= numberOfCommandToTrigger * _commandPeriod;

            if (_commandCounter == _totalCommandCount)
            {
                OnCommandsCompleted?.Invoke();
                Debug.Log("Total time of commands "+(Time.time-bornTime));
                Destroy(gameObject);
            }

        }
    }
}
