
using Managers;
using UnityEngine;

namespace Helpers
{
    public class CameraTargetFollowerSetter : MonoBehaviour
    {
        public CameraStatus[] cameraStatuses;
        public bool setOnStart = true;
        public bool setAsTarget = true;
        public bool setAsLookAt = true;

        // Start is called before the first frame update
        void Start()
        {
            if (setOnStart)
            {
                Set();
            }
        }

        [ContextMenu("Set Camera Targets")]
        private void Set()
        {
            var cameraManager = CameraManager.Instance;
            foreach (var cameraStatus in cameraStatuses)
            {
                if (setAsTarget)
                {
                    cameraManager.SetFollowTarget(cameraStatus.status, transform);
                }

                if (setAsLookAt)
                {
                    cameraManager.SetLookAtTarget(cameraStatus.status, transform);
                }

#if UNITY_EDITOR
                var cam = cameraManager.GetVirtualCamera(cameraStatus.status);
                UnityEditor.EditorUtility.SetDirty(cam);
#endif
            }
        }
    }
}
