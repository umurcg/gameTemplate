using ScriptableObjects;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Editor
{
    public class RebootBuildPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            var rebootSettings = Resources.Load<RebootSettings>("RebootSettings");
            if (!rebootSettings)
            {
                throw new BuildFailedException("Reboot Settings does not exist!");
            }

            bool passedCheck = rebootSettings.ReadyToBuild(out string errorLog);
            if (!passedCheck)
            {
                throw new BuildFailedException(errorLog);
            }
        }
    }
}