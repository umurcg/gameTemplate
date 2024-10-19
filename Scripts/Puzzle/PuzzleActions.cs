using System;

namespace Puzzle
{
    public static class PuzzleActions 
    {
        public static Action OnAdditionalMoveOffer;
        public static Action OnValidMove;
        public static Action OnAdditionalMoveBought;
        public static Action OnPuzzleManagerInitialized;
        public static Action<int> OnMoveCountChanged;
        
        public static void Reset()
        {
            OnAdditionalMoveOffer = null;
            OnValidMove = null;
            OnAdditionalMoveBought = null;
            OnPuzzleManagerInitialized = null;
            OnMoveCountChanged = null;
        }
        
    }
}