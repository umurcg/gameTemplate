#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using CorePublic.Enums;
using UnityEngine;

namespace CorePublic.LevelDesignComponents
{
    [AddComponentMenu("*Reboot/LevelDesign/Rotate Around")]
    public class RotateAround : MonoBehaviour
    {
        [SerializeField]private UpdateMethod _updateMethod;
        
        public float speed = 100f;
        public Vector3 axis = Vector3.forward;
        public Transform pivot;
        public Space space = Space.Self;

        public bool useLimits;
        public Vector2 limits = new Vector2(30, 180);

        private float _angle;
        private bool _increasing = true;
        private Vector3 _realAxis;

        private void Start()
        {
            if (pivot == null) pivot = transform;

            if (space == Space.World)
                _realAxis = space == Space.World ? axis : transform.TransformDirection(axis);

            limits.x = Mathf.Clamp(limits.x, 0, 360);
            if (limits.y < limits.x) limits.y = limits.x;

            // start with lowest limit
            if (useLimits) _angle = limits.x;
        }

        private void Update()
        {
            if(_updateMethod==UpdateMethod.Update)
                UpdateRotation(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if(_updateMethod==UpdateMethod.FixedUpdate)
                UpdateRotation(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if(_updateMethod==UpdateMethod.LateUpdate)
                UpdateRotation(Time.deltaTime);
        }
        
        private void UpdateRotation(float deltaTime)
        {
            if (speed == 0) return;

            if (space == Space.Self)
                _realAxis = space == Space.World ? axis : transform.TransformDirection(axis);

            if (!useLimits)
            {
                transform.RotateAround(pivot.position, _realAxis, speed * deltaTime);
            }
            else
            {
                if (_increasing)
                {
                    _angle += speed * deltaTime;
                    if (_angle > limits.y)
                    {
                        _angle = limits.y;
                        _increasing = false;
                    }
                }
                else
                {
                    _angle -= speed * deltaTime;
                    if (_angle < limits.x)
                    {
                        _angle = limits.x;
                        _increasing = true;
                    }
                }

                transform.rotation = Quaternion.Euler(0, _angle, 0);
            }
        }
    }
}
