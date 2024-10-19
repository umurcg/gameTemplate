using System;
using CorePublic.Managers;
using Lean.Touch;
using UnityEngine;

namespace RebootTemplate.Controllers
{
    public class SimpleRunnerController : MonoBehaviour
    {
        [SerializeField] private float movementRange = 2f;
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private Space movementSpace = Space.World;
        [SerializeField] private UpdateMethods updateMethod = UpdateMethods.Transform;
        [SerializeField] private RotationMethods rotationMethod = RotationMethods.Disabled;
        [SerializeField] private bool clampMaxInput;
        [SerializeField] private float movementSpeed = 2f;

        [SerializeField]
        [Tooltip("Maximum input that will be accepted for per update in pixel unit")]
        private float maxInputDistance = 100;

        [SerializeField] [Range(0, 90)]
        private float maxRotationAngle = 70f;

        [SerializeField]
        private float rotationSensitivity = 1;

        [SerializeField]
        private float rotationDeceleration = 1;

        /// <summary>
        ///     Current rotation in degrees
        /// </summary>
        [SerializeField] private float currentRotationAngle;

#if UNITY_EDITOR
        public KeyCode pauseShortCutKey = KeyCode.P;
#endif
        private CoreManager _coreManager;
        private Vector3 _initialForward;
        private Quaternion _initialRotation;

        private bool _paused;
        private Rigidbody _rigidbody;
        
        
        private bool IsTransformRotation => rotationMethod == RotationMethods.Rotate;

        /// <summary>
        ///     Forward direction of runner
        /// </summary>
        public Vector3 Forward => movementSpace == Space.Self ? _initialForward : Vector3.forward;

        /// <summary>
        ///     Right direction of runner
        /// </summary>
        public Vector3 Right => movementSpace == Space.Self ? Vector3.Cross(Vector3.up, Forward) : Vector3.right;

        private float deltaTime => updateMethod == UpdateMethods.Transform ? Time.deltaTime : Time.fixedTime;


        private void Awake()
        {
            _initialForward = transform.forward;
            _initialRotation = transform.rotation;
        }

        // Start is called before the first frame update
        private void Start()
        {
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            if (updateMethod == UpdateMethods.Physic) _rigidbody = GetComponent<Rigidbody>();
            _coreManager = CoreManager.Request();
        }

        private void OnDestroy()
        {
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_coreManager.IsGameStarted) return;


            MoveForward();
            UpdateRotation();

#if UNITY_EDITOR
            if (Input.GetKeyDown(pauseShortCutKey))
                _paused = !_paused;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movementSpeed += 1;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                movementSpeed -= 1;
            }
#endif
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethods.Physic)
            {
                MoveForward();
                UpdateRotation();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            var right = movementSpace == Space.Self ? Vector3.Cross(Forward, Vector3.up) : Vector3.right;
            var rightLimit = transform.position + right * movementRange / 2;
            var leftLimit = transform.position - right * movementRange / 2;

            float guidlineDistance = 10;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(rightLimit, rightLimit + guidlineDistance * Forward);
            Gizmos.DrawLine(leftLimit, leftLimit + guidlineDistance * Forward);
        }
#endif

        private void OnFingerUpdate(LeanFinger finger)
        {
            if (!_coreManager.IsGameStarted) return;

            var deltaX = finger.ScreenDelta.x;
            if (clampMaxInput && Mathf.Abs(deltaX) > maxInputDistance)
                deltaX = Mathf.Sign(deltaX) * maxInputDistance;

            var aimPos = transform.position + deltaX * Right * sensitivity * Time.deltaTime;
            var aimAlignment = Vector3.Dot(aimPos, Right);
            var currentAlignment = Vector3.Dot(transform.position, Right);

            if (aimAlignment > movementRange / 2 || aimAlignment < -movementRange / 2)
            {
                aimAlignment = movementRange / 2 * Mathf.Sign(aimAlignment);
                aimPos = transform.position - currentAlignment * Right + aimAlignment * Right;
            }

            var deltaDistance = aimAlignment - currentAlignment;
            MoveToPosition(aimPos);
            ChangeRotation(deltaDistance * rotationSensitivity * deltaTime);
        }

        private void MoveToPosition(Vector3 aimPos)
        {
            if (updateMethod == UpdateMethods.Transform)
                transform.position = aimPos;
            else
                _rigidbody.MovePosition(aimPos);
        }

        private void ChangeRotation(float delta)
        {
            currentRotationAngle += delta;
            currentRotationAngle = Mathf.Clamp(currentRotationAngle, -maxRotationAngle, maxRotationAngle);
        }

        private void UpdateRotation()
        {
            if (rotationMethod == RotationMethods.Disabled) return;

            if (rotationMethod == RotationMethods.Rotate)
            {
                var aimRotation = _initialRotation * Quaternion.Euler(0, currentRotationAngle, 0);

                if (updateMethod == UpdateMethods.Transform)
                    transform.rotation = aimRotation;
                else if (updateMethod == UpdateMethods.Physic)
                    _rigidbody.MoveRotation(aimRotation);

                currentRotationAngle = Mathf.MoveTowards(currentRotationAngle, 0, deltaTime * rotationDeceleration);
            }
        }

        private void MoveForward()
        {
            if (_paused) return;

            var aimPos = transform.position + movementSpeed * Forward * deltaTime;

            if (updateMethod == UpdateMethods.Transform)
                transform.position = aimPos;
            else
                _rigidbody.MovePosition(aimPos);
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Continue()
        {
            _paused = false;
        }

        private enum UpdateMethods
        {
            Transform,
            Physic
        }

        private enum RotationMethods
        {
            Disabled,
            Rotate
        }
    }
}