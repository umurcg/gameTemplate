using System;
using Helpers;
using Lean.Touch;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Samples.JoystickUI
{
    public class JoystickController : Singleton<JoystickController>
    {
        public struct JoystickData
        {
            public Vector2 CenterPosition;
            public Vector2 Delta;
            public Vector2 DeltaFromStart;

            /// <summary>
            /// Delta distance from start with magnitude between 0-1. 1 is maximum range of joystick.
            /// </summary>
            public Vector2 NormalizedDeltaFromStart;

            public Vector2 Direction;
            public float ScreenMagnitude;
            public float NormalizedMagnitude;
        }

        private JoystickData _joystickData;

        [SerializeField] [Tooltip("Maximum range for the joystick knob.")]
        private float maximumRange = 1f;

        [SerializeField] [Tooltip("Center of the joystick. This will not move.")]
        private RectTransform center;

        [SerializeField] [Tooltip("Knob of the joystick. This transform moves with the finger in the screen.")]
        private RectTransform knob;

        [SerializeField] private float centerMoveSpeed = 0;

        public Action<JoystickData> OnJoystickUpdate;
        public Action<JoystickData> OnJoystickStart;
        public Action<JoystickData> OnJoystickEnd;

        public bool IsPressing => knob.gameObject.activeSelf;

        public enum BindTypes
        {
            OnStart,
            OnGameStart,
            OnEnable
        }

        public BindTypes type = BindTypes.OnStart;
        private bool _inputBound;

        /// <summary>
        /// Take input actions, and hide the joystick.
        /// </summary>
        void Start()
        {
            if (type == BindTypes.OnStart)
            {
                BindInput();
            }
            else
            {
                GlobalActions.OnGameStarted += BindInput;
                GlobalActions.OnGameEnded += UnbindInput;
            }

            ShowJoystick(false);
        }

        private void OnEnable()
        {
            if (type == BindTypes.OnEnable)
            {
                BindInput();
            }
        }

        private void OnDisable()
        {
            if (type == BindTypes.OnEnable)
            {
                UnbindInput();
            }
        }

        private void BindInput()
        {
            if (_inputBound) return;
            LeanTouch.OnFingerDown += FingerDown;
            LeanTouch.OnFingerUpdate += FingerUpdate;
            LeanTouch.OnFingerUp += FingerUp;
            _inputBound = true;
        }

        private void UnbindInput()
        {
            if (!_inputBound) return;
            LeanTouch.OnFingerDown -= FingerDown;
            LeanTouch.OnFingerUpdate -= FingerUpdate;
            LeanTouch.OnFingerUp -= FingerUp;
            _inputBound = false;
        }

        /// <summary>
        /// When player touches the screen, set joystick position as screen position and activate the joystick.
        /// </summary>
        /// <param name="finger"></param>
        void FingerDown(LeanFinger finger)
        {
            if (!Application.isEditor && finger.Index != 0) return;

            Vector2 position = finger.ScreenPosition;
            center.position = position;
            knob.position = position;
            ShowJoystick(true);

            _joystickData.CenterPosition = position;
            _joystickData.Delta = Vector2.zero;
            _joystickData.DeltaFromStart = Vector2.zero;
            _joystickData.NormalizedDeltaFromStart = Vector2.zero;
            _joystickData.Direction = Vector2.zero;
            _joystickData.ScreenMagnitude = 0f;
            _joystickData.NormalizedMagnitude = 0f;

            OnJoystickStart?.Invoke(_joystickData);
        }


        /// <summary>
        /// When player moves the finger, move joystick knob with a clamp in maximum range. Also set the direction to use from anywhere.
        /// </summary>
        /// <param name="finger"></param>
        void FingerUpdate(LeanFinger finger)
        {
            if (!Application.isEditor && finger.Index != 0) return;

            float range = maximumRange * center.sizeDelta.x;

            var centerToFingerPosition = (Vector3)finger.ScreenPosition - center.position;
            //If touch position is outside the joystick range, move the center of the joystick to the range limit.
            if (centerToFingerPosition.magnitude > range)
            {
                var height = Screen.height;
                var pixelSpeed = centerMoveSpeed * height;

                var aimPos = (Vector3)finger.ScreenPosition - centerToFingerPosition.normalized * range;
                center.position = Vector3.MoveTowards(center.position, aimPos, pixelSpeed * Time.deltaTime);
            }

            knob.position = center.position +
                            Vector3.ClampMagnitude((Vector3)finger.ScreenPosition - center.position, range);

            _joystickData.Delta = finger.ScreenDelta;
            _joystickData.DeltaFromStart = knob.position - (Vector3)_joystickData.CenterPosition;
            _joystickData.NormalizedDeltaFromStart = _joystickData.DeltaFromStart / range;
            _joystickData.Direction = _joystickData.NormalizedDeltaFromStart.normalized;
            _joystickData.ScreenMagnitude = _joystickData.DeltaFromStart.magnitude;
            _joystickData.NormalizedMagnitude = _joystickData.NormalizedDeltaFromStart.magnitude;

            OnJoystickUpdate?.Invoke(_joystickData);
        }

        /// <summary>
        /// Deactivate joystick
        /// </summary>
        /// <param name="finger"></param>
        void FingerUp(LeanFinger finger)
        {
            if (!Application.isEditor && finger.Index != 0) return;

            ShowJoystick(false);
            OnJoystickEnd?.Invoke(_joystickData);
        }

        /// <summary>
        /// Allows to activate or deactivate the joystick.
        /// </summary>
        /// <param name="activate"></param>
        private void ShowJoystick(bool activate)
        {
            knob.gameObject.SetActive(activate);
            center.gameObject.SetActive(activate);
        }

        /// <summary>
        /// Delete actions.
        /// </summary>
        private void OnDestroy()
        {
            if (type == BindTypes.OnGameStart)
            {
                LeanTouch.OnFingerDown -= FingerDown;
                LeanTouch.OnFingerUpdate -= FingerUpdate;
                LeanTouch.OnFingerUp -= FingerUp;
            }
        }
    }
}