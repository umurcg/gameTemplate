using UnityEngine;
using UnityEngine.Events;

namespace Puzzle.UI
{
    public class MoveCounterUI : MonoBehaviour
    {
        private TMPro.TMP_Text moveCounter;
        public UnityEvent onCountChanged;
        private bool _initialized;

        private void Initialize()
        {
            if (_initialized) return;
            if (moveCounter == null) moveCounter = GetComponent<TMPro.TMP_Text>();

            PuzzleActions.Instance.OnMoveCountChanged += OnMovementCountChanged;
            moveCounter.text = PuzzleManager.Instance.LeftMoveCount.ToString();
            _initialized = true;
        }

        private void Start()
        {
            if (!_initialized)
            {
                Initialize();
            }
        }

        private void OnEnable()
        {
            if (!_initialized)
            {
                Initialize();
            }
        }

        private void OnDisable()
        {
            if (!_initialized)
            {
                return;
            }

            if (PuzzleActions.Instance == null) return;
            PuzzleActions.Instance.OnMoveCountChanged -= OnMovementCountChanged;
            _initialized = false;
        }

        private void OnMovementCountChanged(int leftMoveCount)
        {
            moveCounter.text = leftMoveCount.ToString();
            onCountChanged?.Invoke();
        }
    }
}