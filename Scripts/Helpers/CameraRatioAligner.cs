using Cinemachine;
using CorePublic.Managers;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class CameraRatioAligner : MonoBehaviour
    {
        [SerializeField] private float ratio = 1f;
        [SerializeField] private Transform aim;
        private CinemachineVirtualCamera _camera;
        private float _initialX;

        private CinemachineTransposer _transposer;
        private CinemachineComposer _composer;

        private CoreManager _coreManager;

        private void Start()
        {
            _coreManager = CoreManager.Request();
            GlobalActions.OnGameStarted += GameStarted;


        }

        void GameStarted()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            if (_camera.m_Follow != _camera.m_LookAt)
            {
                Debug.LogError("Camera follow and look at target must be equal");
                Destroy(this);
                return;
            }

            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _composer = _camera.GetCinemachineComponent<CinemachineComposer>();

            aim = _camera.m_Follow;
            _initialX = aim.position.x;
        }

        private void Update()
        {
            if (!_coreManager.IsGameStarted) return;
        
            float xDif = aim.transform.position.x-_initialX;
            float factoredDif = xDif * ratio;
            float offset = -(xDif - factoredDif);

            _transposer.m_FollowOffset.x = offset;
            _composer.m_TrackedObjectOffset.x = offset;
        
        }

        public void SetAim(Transform aim)
        {
            this.aim = aim;
        }
    }
}

