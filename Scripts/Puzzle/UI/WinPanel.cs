using System.Collections;
using Managers;
using UnityEngine;

namespace Puzzle.UI
{
    public class WinPanel: UIPanel
    {
        [SerializeField] private float convertMaxRate = 0.1f;
        [SerializeField] private float convertMaxDurationFlow = .8f;
        public float convertMoveAfterActivation=1f;
        private bool _isLoadingNextLevel = false;
        
        public void Start()
        {
            ActionManager.Instance.OnGameWin += Activate;
            ActionManager.Instance.OnLevelDestroyed += Deactivate;
            OnDeactivated += () => _isLoadingNextLevel = false;
        }
        
        public new void Activate()
        {
            StartCoroutine(ActivateCoroutine());
        }

        private IEnumerator ActivateCoroutine()
        {
            yield return StartCoroutine(base.ActivateCoroutine());
            yield return new WaitForSeconds(convertMoveAfterActivation);
            StartCoroutine(PuzzleManager.Instance.ConvertMovesToCurrencyCoroutine(convertMaxDurationFlow,convertMaxRate,SetAnimationTrigger));
        }
        
        public void NextLevel()
        {
            if (_isLoadingNextLevel) return;
            SetAnimationTrigger();
            CoreManager.Instance.IncreaseLevel();
            _isLoadingNextLevel = true;
            
        }
        
    

    }
}