using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CorePublic.Managers
{
    [Serializable]public class CameraStatus
    {
#if ODIN_INSPECTOR
    [ValueDropdown("GetCameraStatuses")]
#endif
        public string status;
        
        public string[] GetCameraStatuses()
        {
            CinemachineManager cameraManager = CinemachineManager.Instance;
            if (!cameraManager)
                return null;

            return cameraManager.GetCameraStatuses();
        }
    }
}