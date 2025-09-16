using UnityEngine;
using CorePublic.Managers;
namespace CorePublic.Helpers
{
    public class GamePauser: MonoBehaviour
    {
        public void PauseGame()
        {
            if(CoreManager.Instance.GameState == GameStates.InGame)
            {
                CoreManager.Instance.PauseGame();
            }
            
        }

        public void ResumeGame()
        {
            if(CoreManager.Instance.GameState == GameStates.Pause)
            {
                CoreManager.Instance.ResumeGame();
            }
        }
    }
}

