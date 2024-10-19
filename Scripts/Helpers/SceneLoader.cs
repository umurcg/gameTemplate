using System.Collections;
using CorePublic.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePublic.Helpers
{
    public class SceneLoader : MonoBehaviour
    {
        public PrerequisiteReference[] Prerequisites;
        public bool loadOnStart;
        public int sceneIndex;
        public bool loadAsync;
    
        public void Start()
        {
            if (loadOnStart)
            {
                StartCoroutine(LoadScene());
            }
        }

        private IEnumerator LoadScene()
        {
            //Wait for all prerequisites to be met
            while (!IsReady()) yield return null;
        
            if (loadAsync)
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
                while (!asyncOperation.isDone) yield return null;
                SceneManager.UnloadSceneAsync(currentSceneIndex);
            
            }else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    
        public void LoadScene(int index)
        {
            if(!IsReady()) return;
            SceneManager.LoadScene(index-1);
        }
    
 
        private bool IsReady()
        {
            foreach (PrerequisiteReference prerequisiteRef in Prerequisites)
            {
                var prerequisite = prerequisiteRef.Prerequisite;
                if (prerequisite == null) continue;
                if (!prerequisite.IsMet()) return false;
            }
            return true;
        }

    }
}
