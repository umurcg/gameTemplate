using System.Collections;
using Managers;
using UnityEngine;

namespace UI
{
    public class EndUIController : UIElement
    {
        [SerializeField]private float panelDelay=2f;
        [SerializeField]private UIElement failPanel;
        [SerializeField]private UIElement winPanel;


        public override void Initialize()
        {
            base.Initialize();
            ActionManager.Instance.OnGameLost+=(LevelFailed);
            ActionManager.Instance.OnGameWin+=(LevelWin);
            ActionManager.Instance.OnNewLevelLoaded += Deactivate;

        }

        private void OnDestroy()
        {
            if(!ActionManager.Instance) return;
            ActionManager.Instance.OnGameLost-=(LevelFailed);
            ActionManager.Instance.OnGameWin-=(LevelWin);
        }
        
        private void LevelFailed()
        {
            StartCoroutine(ActivatePanel(failPanel));
            Activate();
        }
        
        private void LevelWin()
        {
            StartCoroutine(ActivatePanel(winPanel));
            Activate();
        }

        private IEnumerator ActivatePanel(UIElement panel)
        {
            yield return new WaitForSeconds(panelDelay);
            panel.SetActive(true);
        }

        public void NextLevel()
        {
            Deactivate();
            CoreManager.IncreaseLevel();
        }

        public void RestartLevel()
        {
            Deactivate();
            CoreManager.ReloadLevel();
        }
    }
}