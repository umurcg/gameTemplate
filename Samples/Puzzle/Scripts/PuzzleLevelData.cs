using ScriptableObjects;
using UnityEngine;

namespace Samples.Puzzle.Scripts
{
    [CreateAssetMenu(fileName = "PuzzleLevelData", menuName = "Puzzle/LevelData")]
    public class PuzzleLevelData: LevelData
    {
        public int moveCount;
    }
}