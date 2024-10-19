using CorePublic.ScriptableObjects;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(fileName = "PuzzleLevelData", menuName = "Puzzle/LevelData")]
    public class PuzzleLevelData: LevelData
    {
        public int moveCount;
    }
}