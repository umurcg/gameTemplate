using CorePublic.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePublic.Editor
{
    public static class RebootEditor
    {
        [MenuItem ("Tools/Revert to Prefab %r")]
        public static void Revert() {
            GameObject[] selection = Selection.gameObjects;
 
            if (selection.Length > 0) {
                for (int i = 0; i < selection.Length; i++)
                {
                    GameObject obj = selection[i] as GameObject;
                    PrefabUtility.RevertObjectOverride(obj, InteractionMode.UserAction);
                }
            } else {
                Debug.Log("Cannot revert to prefab - nothing selected");
            }
        }

        [MenuItem("Reboot/Edit Settings")]
        public static void EditSettings()
        {
            var settings = Resources.Load<RebootSettings>("RebootSettings");

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<RebootSettings>();

                var path = "Assets/Resources";

                if (!AssetDatabase.IsValidFolder(path))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/RebootSettings.asset");

                AssetDatabase.CreateAsset(settings, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
        }
        
        [MenuItem("Reboot/Clear Save Data %#c")]
        public static void ClearSaveData()
        {
            Debug.Log("Clearing save data");
            PlayerPrefs.DeleteAll();
        }
        
                
        [MenuItem("Reboot/Reset Scene %#r")]
        public static void ResetScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        
        [MenuItem("Reboot/Increase Time Scale %#RIGHT")]
        public static void IncreaseTimeScale()
        {
            float delta = 1;
            if(Time.timeScale<1)
                delta = .1f;
            
            Time.timeScale += delta;
            Time.fixedDeltaTime = .02f * Time.timeScale;
            Debug.Log("Time scale increased to " + Time.timeScale);
        }
        
        [MenuItem("Reboot/Decrease Time Scale %#LEFT")]
        public static void DecreaseTimeScale()
        {
            float delta = 1;
            if(Time.timeScale<=1)
                delta = .1f;
            
            Time.timeScale -= delta;
            Time.fixedDeltaTime = .02f * Time.timeScale;
            Debug.Log("Time scale decreased to " + Time.timeScale);
        }

        [MenuItem("Reboot/Switch Fast Mode %#x")]
        public static void SwitchEditorFastStartMode()
        {
            if (EditorSettings.enterPlayModeOptions == EnterPlayModeOptions.None)
            {
                EditorSettings.enterPlayModeOptionsEnabled = true;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload |
                                                      EnterPlayModeOptions.DisableSceneReload;
                
                
            }
            else
            {
                EditorSettings.enterPlayModeOptionsEnabled = false;
                EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;
                
            }
            
            Debug.Log("Editor fast mode enabled: "+EditorSettings.enterPlayModeOptionsEnabled);
        }
    }
    
    
}