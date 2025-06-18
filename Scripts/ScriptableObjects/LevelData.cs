using System;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Reboot/LevelData", order = 1), Serializable]
    public class LevelData : ScriptableObject
    {
        public string LevelName => name;
        public GameObject LevelPrefab;
        
        /// <summary>
        /// If checked, level will be skipped if the levelfunnel is being repeated.
        /// </summary>
        public bool SkipOnRepeat = false;

#if UNITY_EDITOR

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#else
        [ContextMenu("Play Level")]
#endif
        public void PlayLevel()
        {
            //Start editor play mode
            UnityEditor.EditorApplication.isPlaying = true;
            var levelManager = FindObjectOfType<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("Level Manager not found in scene");
                return;
            }
            levelManager.TestLevelData = this;

        }
#endif
    }
}