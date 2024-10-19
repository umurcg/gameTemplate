using System.Linq;
using Cinemachine;
using CorePublic.Helpers;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Utilities;
#endif


namespace CorePublic.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CinemachineVirtualCamera[] cameras;

        private const int ACTIVE_VALUE = 10;
        private const int PASSIVE_VALUE = 1;

#if ODIN_INSPECTOR
        [ValueDropdown("GetCameraStatuses")]
#endif
        public string defaultStatus;


        [HideInInspector] public CinemachineBrain brain;

        public string CurrentStatus { get; private set; }
        public Camera MainCamera { get; private set; }

        private void Start()
        {
            MainCamera = Camera.main;
            if (MainCamera != null) brain = MainCamera.GetComponent<CinemachineBrain>();

            foreach (CinemachineVirtualCamera virtualCamera in cameras)
            {
                if (virtualCamera.name == "StartCamera")
                {
                    GlobalActions.OnNewLevelLoaded += () => SetCameraStatus("StartCamera");
                }
                else if (virtualCamera.name == "InGameCamera")
                {
                    GlobalActions.OnGameStarted += () => SetCameraStatus("InGameCamera");
                }
                else if (virtualCamera.name == "WinCamera")
                {
                    GlobalActions.OnGameWin += () => SetCameraStatus("WinCamera");
                }
                else if (virtualCamera.name == "LostCamera")
                {
                    GlobalActions.OnGameLost += () => SetCameraStatus("LostCamera");
                }
            }

            SetCameraStatus(defaultStatus);
        }

        public void SetCameraStatus(string status)
        {
            foreach (CinemachineVirtualCamera virtualCamera in cameras)
            {
                if (virtualCamera.name == status)
                {
                    virtualCamera.Priority = ACTIVE_VALUE;
                }
                else
                {
                    virtualCamera.Priority = PASSIVE_VALUE;
                }
            }

            CurrentStatus = status;
        }

        /// <summary>
        ///     Sets the follow target for given status virtual camera.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public void SetFollowTarget(string status, Transform target)
        {
            foreach (CinemachineVirtualCamera virtualCamera in cameras)
            {
                if (virtualCamera.name == status)
                {
                    virtualCamera.Follow = target;
                }
            }
        }

        /// <summary>
        ///     Sets the look at target for given status virtual camera.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public void SetLookAtTarget(string status, Transform target)
        {
            foreach (CinemachineVirtualCamera virtualCamera in cameras)
            {
                if (virtualCamera.name == status)
                {
                    virtualCamera.LookAt = target;
                }
            }
        }

        public CinemachineVirtualCamera GetVirtualCamera(string status)
        {
            return cameras
                .Where(vc => vc.name == status)
                .Select(vc => vc)
                .FirstOrDefault();
        }


        /// <summary>
        ///     Returns whether or not world position can be seen by main camera.
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public static bool IsPositionInCameraField(Vector3 worldPos)
        {
            if (Camera.main == null) return false;
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            return screenPos.x < Screen.width && screenPos.x > 0 && screenPos.y < Screen.height && screenPos.y > 0;
        }

        public string[] GetCameraStatuses()
        {
            return cameras.Select(vc => vc.name).ToArray();
        }
    }
}