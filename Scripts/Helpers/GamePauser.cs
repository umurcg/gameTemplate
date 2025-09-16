using UnityEngine;
using CorePublic.Managers;
namespace CorePublic.Helpers
{
    public class GamePauser: MonoBehaviour
    {
        public void PauseGame()
        {
            CoreManager.Instance.PauseGame();
        }

        public void ResumeGame()
        {
            CoreManager.Instance.ResumeGame();
        }
    }
}

