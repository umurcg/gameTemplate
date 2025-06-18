using CorePublic.Helpers;
using CorePublic.Managers;
using UnityEngine;
using System.Linq;

namespace CorePublic.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelFunnel", menuName = "Reboot/LevelFunnel", order = 1)]
    public class LevelFunnel : GlobalSO<LevelFunnel>
    {
        public bool RandomizeLevelsAfterFunnelFinished = false;
        public int DefaultRepeatStartLevelIndex = -1;

        public LevelData[] Levels;

        public LevelData GetLevel(int index)
        {
            return Levels[index];
        }

        [ContextMenu("Open Level Rename Tool")]
        private void OpenLevelRenameTool()
        {
#if UNITY_EDITOR
            // Use reflection to avoid direct dependency on editor scripts
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.EditorWindow));
            var editorAssembly = System.AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Contains("CorePublic") && a.GetName().Name.Contains("Editor"));
            
            if (editorAssembly != null)
            {
                var toolType = editorAssembly.GetType("CorePublic.Editor.LevelFunnelRenameTool");
                if (toolType != null)
                {
                    var getWindowMethod = typeof(UnityEditor.EditorWindow).GetMethod("GetWindow", new[] { typeof(System.Type), typeof(string) });
                    var window = getWindowMethod.Invoke(null, new object[] { toolType, "Level Funnel Rename Tool" });
                    
                    if (window != null)
                    {
                        var funnelField = toolType.GetField("selectedLevelFunnel");
                        funnelField?.SetValue(window, this);
                        
                        var getLevelsMethod = toolType.GetMethod("GetLevelsFromLevelFunnel");
                        getLevelsMethod?.Invoke(window, null);
                        
                        var focusMethod = toolType.GetMethod("Focus");
                        focusMethod?.Invoke(window, null);
                    }
                }
            }
#endif
        }

        protected override void LoadSettings()
        {
            var json = RemoteConfig.Instance.GetJson(RemoteConfigKey);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Level funnel is not set");
                return;
            }

            //Just get RandomizeLevelsAfterFunnelFinished and DefaultRepeatStartLevelIndex from json
            var remoteFunnel = JsonUtility.FromJson<LevelFunnel>(json);
            if (remoteFunnel == null)
            {
                Debug.LogError("Level funnel library is not valid");
                return;
            }
            RandomizeLevelsAfterFunnelFinished = remoteFunnel.RandomizeLevelsAfterFunnelFinished;
            DefaultRepeatStartLevelIndex = remoteFunnel.DefaultRepeatStartLevelIndex;
        }
    }
}
