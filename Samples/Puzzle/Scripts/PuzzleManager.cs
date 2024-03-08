using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.Interfaces;
using Core.Managers;
using Helpers;
using Managers;
using Samples.Puzzle.Scripts;
using UnityEngine;

namespace Samples.Puzzle
{
    public class PuzzleManager : Singleton<PuzzleManager>, IStats
    {
        public int LeftMoveCount { get; private set; }
        public PuzzleLevelData ActiveLevelData { get; private set; }

        public int LostCounter
        {
            get => PlayerPrefs.GetInt("LostCounter", 0);
            private set => PlayerPrefs.SetInt("LostCounter", value);
        }

        public int AdditionalMoveCount = 5;
        public int AdditionalMoveCost = 100;
        [SerializeField] private int MoveCountValue = 2;
        
        private bool _clearLostData;
        private bool _didUseAdditionalMove;
        
        public PrerequisiteReference[] lostPrerequisites;
        
        public new void Awake()
        {
            base.Awake();
            
            
        }

        private void Start()
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

            if (PuzzleActions.Instance)
            {
                PuzzleActions.Instance.OnValidMove += OnValidMove;
            }
            else
            {
                Debug.LogError("PuzzleActions is missing");
            }
        }

        
        private void OnNewLevelLoaded()
        {
            if (_clearLostData)
            {
                _clearLostData = false;
                PlayerPrefs.SetInt("LostCounter", 0);
            }

            _didUseAdditionalMove = false;
            
            ActiveLevelData = LevelManager.Instance.ActiveLevelData as PuzzleLevelData;
            LeftMoveCount = ActiveLevelData.moveCount;
            
            PuzzleActions.Instance?.OnMoveCountChanged?.Invoke(LeftMoveCount);
            PuzzleActions.Instance?.OnPuzzleManagerInitialized?.Invoke();
        }


        private void OnGameWin()
        {
            _clearLostData = true;
        }

        private void OnGameLost()
        {
            LostCounter++;
        }
        
        private void OnValidMove()
        {
            LeftMoveCount--;
            PuzzleActions.Instance?.OnMoveCountChanged?.Invoke(LeftMoveCount);
            if (LeftMoveCount <= 0)
            {
                StartCoroutine(CallLost());
            }
        }

        public void FakeMove()
        {
            PuzzleActions.Instance?.OnValidMove?.Invoke();
        }

        public void FakeLost()
        {
            LeftMoveCount = 1;
            PuzzleActions.Instance?.OnValidMove?.Invoke();
        }
        

        private IEnumerator CallLost()
        {
            PuzzleActions.Instance?.LostIsCalled?.Invoke();
            yield return StartCoroutine(WaitForLost());

            if (!CoreManager.Instance.IsGameStarted)
            {
                yield break;
            }

            if (!CanBuyMoves())
            {
                CoreManager.Instance.LostGame();
            }
            else
            {
                PuzzleActions.Instance?.OnAdditionalMoveOffer?.Invoke();
            }
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
        
        public int GetScore()
        {
            return LeftMoveCount;
        }

        public bool CanBuyMoves()
        {
            if (_didUseAdditionalMove) return false;
            return CoreManager.Instance.GameMoney >= AdditionalMoveCost;
        }

        public void BuyAdditionalMoves()
        {
            if (CanBuyMoves())
            {
                CoreManager.Instance.SpendMoney(AdditionalMoveCost);
                LeftMoveCount += AdditionalMoveCount;
                _didUseAdditionalMove = true;
                
                PuzzleActions.Instance.OnMoveCountChanged?.Invoke(LeftMoveCount);
                PuzzleActions.Instance.OnAdditionalMoveBought?.Invoke();
            }
        }


        public IEnumerator ConvertMovesToCurrencyCoroutine(float duration, float maxRate, Action onComplete)
        {
            float durationPerMove = duration / LeftMoveCount;
            if (durationPerMove > maxRate)
            {
                durationPerMove = maxRate;
            }
            
            while (LeftMoveCount > 0)
            {
                LeftMoveCount--;
                PuzzleActions.Instance?.OnMoveCountChanged?.Invoke(LeftMoveCount);
                CoreManager.Instance.EarnMoney(MoveCountValue);
                yield return new WaitForSeconds(durationPerMove);
            }

            onComplete?.Invoke();
        }
        
        public string GetStats()
        {
            var stats = "";

            if (ActiveLevelData != null)
            {
                stats += "Level: " + ActiveLevelData.levelName + "\n";
                stats += "Move count: " + LeftMoveCount + "\n";
                stats += "Lost counter: " + LostCounter + "\n";
                stats += "Money: " + CoreManager.Instance.GameMoney + "\n";
            }

            return stats;
        }
    }
}