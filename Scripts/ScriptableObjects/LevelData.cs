using System;
using Managers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Reboot/LevelData", order = 1),Serializable]
    public class LevelData : ScriptableObject
    {
        public string levelName=> name;
        public GameObject levelPrefab;

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
            var levelManager=FindObjectOfType<LevelManager>();
            if (levelManager == null)
            {
                Debug.LogError("Level Manager not found in scene");
                return;
            }
            levelManager.TestLevelData=this;
            
        }
#endif
    }
}