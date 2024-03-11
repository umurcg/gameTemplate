using System;
using System.Collections;
using Core.Interfaces;
using Helpers;
using Managers;
using UnityEngine;

namespace Puzzle
{
    [RequireComponent(typeof(PuzzleManager))]
    public class MoveManager: Singleton<MoveManager>, IStats
    {
        public int LeftMoveCount { get; private set; }
        public int AdditionalMoveCount = 5;
        public int AdditionalMoveCost = 100;
        [SerializeField] private int MoveCountValue = 2;

        private PuzzleManager _puzzleManager;
        private bool _didUseAdditionalMove;
        
        void Start()
        {
            _puzzleManager = GetComponent<PuzzleManager>();
            _puzzleManager.CanCallGameLost = false;
            _puzzleManager.LostCallFinished += OnLostCallFinished;
            

            if (ActionManager.Instance)
            {
                ActionManager.Instance.OnNewLevelLoaded += OnNewLevelLoaded;
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
            var level = LevelManager.Instance.ActiveLevelData as PuzzleLevelData;
            LeftMoveCount = level.moveCount;
            PuzzleActions.Instance?.OnMoveCountChanged?.Invoke(LeftMoveCount);
            _didUseAdditionalMove = false;
        }
        
        private void OnValidMove()
        {
            LeftMoveCount--;
            PuzzleActions.Instance?.OnMoveCountChanged?.Invoke(LeftMoveCount);
            if (LeftMoveCount <= 0)
            {
                StartCoroutine(_puzzleManager.CallLost());
            }
        }


        private void OnLostCallFinished()
        {
            if (!CanBuyMoves())
            {
                CoreManager.Instance.LostGame();
            }
            else
            {
                PuzzleActions.Instance?.OnAdditionalMoveOffer?.Invoke();
            }
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

        public void FakeMove()
        {
            PuzzleActions.Instance?.OnValidMove?.Invoke();
        }

        public void FakeLost()
        {
            LeftMoveCount = 1;
            PuzzleActions.Instance?.OnValidMove?.Invoke();
        }


        public string GetStats()
        {
            var stats = "Move count: " + LeftMoveCount + "\n";
            return stats;
        }
    }
}