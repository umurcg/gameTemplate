using CorePublic.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace CorePublic.UI
{
    /// <summary>
    ///     Used for switching between levels easily. Do not use this approach to load new levels it is only for debug purposes
    /// </summary>
    public class DebugPanelController : MonoBehaviour
    {
        public Text debugLevelLabel;
        private CoreManager _coreManager;
        private LevelManager _levelManager;

        // Start is called before the first frame update
        private void Start()
        {
            _coreManager = CoreManager.Request();
            _levelManager = LevelManager.Request();
            debugLevelLabel.text = _coreManager.Level.ToString();
        }


        public void ResetGameData()
        {
            if (_coreManager.IsGameStarted) _coreManager.WinGame();
            CoreManager.ClearSaveData();
            CoreManager.SetLevel(0);
            debugLevelLabel.text = 0.ToString();
        }

        public void IncreaseDebugLevelField()
        {
            if (int.TryParse(debugLevelLabel.text, out var result)) debugLevelLabel.text = (result + 1).ToString();
        }

        public void DecreaseDebugLevelField()
        {
            if (int.TryParse(debugLevelLabel.text, out var result)) debugLevelLabel.text = (result - 1).ToString();
        }

        public void GoDebugLevelField()
        {
            if (int.TryParse(debugLevelLabel.text, out var result))
            {
                if (_coreManager.IsGameStarted) _coreManager.WinGame();
                CoreManager.SetLevel(result);
            }
        }
    }
}