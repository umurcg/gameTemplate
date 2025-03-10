using System;
using System.Collections;
using CorePublic.Helpers;
using CorePublic.Interfaces;
using CorePublic.Managers;
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

            GlobalActions.OnNewLevelLoaded += OnNewLevelLoaded;
            PuzzleActions.OnValidMove += OnValidMove;
           
        }
        
        private void OnNewLevelLoaded()
        {
            var level = LevelManager.Instance.ActiveLevelData as PuzzleLevelData;
            LeftMoveCount = level.moveCount;
            PuzzleActions.OnMoveCountChanged?.Invoke(LeftMoveCount);
            _didUseAdditionalMove = false;
        }
        
        private void OnValidMove()
        {
            LeftMoveCount--;
            PuzzleActions.OnMoveCountChanged?.Invoke(LeftMoveCount);
            if (LeftMoveCount <= 0)
            {
                StartCoroutine(_puzzleManager.CallLost());
            }
        }


        private void OnLostCallFinished()
        {
            if (!CanBuyMoves())
            {
                CoreManager.Instance.LostGame(_puzzleManager.LostReason);
            }
            else
            {
                PuzzleActions.OnAdditionalMoveOffer?.Invoke();
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
                
                PuzzleActions.OnMoveCountChanged?.Invoke(LeftMoveCount);
                PuzzleActions.OnAdditionalMoveBought?.Invoke();
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
                PuzzleActions.OnMoveCountChanged?.Invoke(LeftMoveCount);
                CoreManager.Instance.EarnMoney(MoveCountValue);
                yield return new WaitForSeconds(durationPerMove);
            }

            onComplete?.Invoke();
        }

        public void FakeMove()
        {
            PuzzleActions.OnValidMove?.Invoke();
        }

        public void FakeLost()
        {
            LeftMoveCount = 1;
            PuzzleActions.OnValidMove?.Invoke();
        }


        public string GetStats()
        {
            var stats = "Move count: " + LeftMoveCount + "\n";
            return stats;
        }
    }
}