using CorePublic.ScriptableObjects;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CorePublic.Editor
{
    public class RebootBuildPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            var rebootSettings = RebootSettings.Instance;
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