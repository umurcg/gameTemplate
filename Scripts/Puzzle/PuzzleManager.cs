using System;
using System.Collections;
using System.Linq;
using Core.Interfaces;
using Helpers;
using Managers;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleManager : Singleton<PuzzleManager>, IStats
    {
      
        public PuzzleLevelData ActiveLevelData { get; private set; }

        public int LostCounter
        {
            get => PlayerPrefs.GetInt("LostCounter", 0);
            private set => PlayerPrefs.SetInt("LostCounter", value);
        }

     
        private bool _clearLostData;

        public PrerequisiteReference[] lostPrerequisites;

        public bool CanCallGameLost;

        public Action LostCallFinished;
        
        public new void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            if (ActionManager.Instance)
            {
                ActionManager.Instance.OnNewLevelLoaded += OnNewLevelLoaded;
                ActionManager.Instance.OnGameLost += OnGameLost;
                ActionManager.Instance.OnGameWin += OnGameWin;
            }
            else
            {
                Debug.LogError("ActionManager is missing");
            }
        }

        
        protected void OnNewLevelLoaded()
        {
            if (_clearLostData)
            {
                _clearLostData = false;
                PlayerPrefs.SetInt("LostCounter", 0);
            }
            
            ActiveLevelData = LevelManager.Instance.ActiveLevelData as PuzzleLevelData;
            PuzzleActions.Instance?.OnPuzzleManagerInitialized?.Invoke();
        }


        protected void OnGameWin()
        {
            _clearLostData = true;
        }

        protected void OnGameLost()
        {
            LostCounter++;
        }
        
        public IEnumerator CallLost()
        {
            yield return StartCoroutine(WaitForLost());

            if (!CoreManager.Instance.IsGameStarted)
            {
                yield break;
            }
            
            LostCallFinished?.Invoke();
            if (CanCallGameLost) CoreManager.Instance.LostGame();
        }

        private IEnumerator WaitForLost()
        {
            float waitDurationBetweenChecks = 0.1f;
            
            //Wait until all prerequisites are met. Use Linq to check all prerequisites
            while (lostPrerequisites.Any(prerequisiteReference => !prerequisiteReference.Prerequisite.IsMet()))
            {
                yield return new WaitForSeconds(waitDurationBetweenChecks);
                if (!CoreManager.Instance.IsGameStarted)
                {
                    yield break;
                }
            }
        }

        public string GetStats()
        {
            var stats = "";

            if (ActiveLevelData != null)
            {
                stats += "Level: " + ActiveLevelData.levelName + "\n";
                stats += "Lost counter: " + LostCounter + "\n";
                stats += "Money: " + CoreManager.Instance.GameMoney + "\n";
            }

            return stats;
        }
    }
}