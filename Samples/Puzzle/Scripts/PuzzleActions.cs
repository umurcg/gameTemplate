using System;
using Core.Interfaces;
using Helpers;
using UnityEngine;

namespace Samples.Puzzle.Scripts
{
    public class PuzzleActions: Singleton<PuzzleActions>, IAction
    {
        public Action OnAdditionalMoveOffer;
        public Action OnValidMove;
        public Action OnAdditionalMoveBought;
        public Action OnPuzzleManagerInitialized;
        public Action<int> OnMoveCountChanged;
        public Action LostIsCalled;
    }
}