
using UnityEngine;

namespace Helpers
{
    [AddComponentMenu("*Reboot/LevelDesign/Dynamic Obstacle")]
    public class DynamicObstacle : MonoBehaviour
    {
        public enum Types
        {
            Movement,
            Rotation
        }

        public Types movementType = Types.Movement;
        public bool reversed;

        [Header("Movement Variables")] public float movementDistance = 10f;

        public float movementSpeed = 3f;
        public Vector3 movementAxis = Vector3.right;
        public Space movementSpace = Space.World;

        [Range(0, 1)] public float movementRatio;


        [Header("Rotation Variables")] public bool pingPong;

        public Vector3 orbitDirection = Vector3.up;
        public float rotateAngle = 30f;
        public float orbitDistance = 5f;
        public float rotateSpeed = 1f;
        public bool lookAtOrbit;
        public float angle;

        private Vector3 _initialPosition;
        private Vector3 _orbitCenter;
        private float _timeOffset;

        // Start is called before the first frame update
        private void Start()
        {
            _orbitCenter = transform.position + orbitDirection * orbitDistance;
            _initialPosition = transform.position;

            movementAxis = movementAxis.normalized;

            if (movementType == Types.Rotation)
            {
                _timeOffset = Mathf.Asin(angle / (rotateAngle / 2));
                if (reversed)
                    _timeOffset -= Mathf.PI;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (movementType == Types.Movement)
                UpdatePosition();
            else
                UpdateRotation();
        }


        private void OnDrawGizmos()
        {
#if UNITY_EDITOR

            if (Application.isPlaying == false)
                _initialPosition = transform.position;

            if (movementType == Types.Rotation)
            {
                if (!pingPong)
                    rotateAngle = 360;
                else
                    angle = Mathf.Clamp(angle, -rotateAngle / 2, rotateAngle / 2);

                if (orbitDistance <= 0)
                    orbitDistance = 0.001f;

                _orbitCenter = _initialPosition + orbitDirection * orbitDistance;
                UnityEditor.Handles.DrawWireArc(_orbitCenter, Vector3.forward,
                    Quaternion.AngleAxis(-rotateAngle / 2, Vector3.forward) * (-1 * orbitDirection), rotateAngle,
                    orbitDistance);
                Gizmos.DrawSphere(
                    _orbitCenter - Quaternion.AngleAxis(angle, Vector3.forward) * orbitDirection * orbitDistance, 0.5f);
            }
            else
            {
                movementAxis = movementAxis.normalized;

                var movAxis = movementSpace == Space.World ? movementAxis : transform.TransformDirection(movementAxis);

                var p1 = _initialPosition + movAxis * movementDistance / 2;
                var p2 = _initialPosition - movAxis * movementDistance / 2;
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawSphere(Vector3.Lerp(p1, p2, movementRatio), 0.5f);
            }

#endif
        }

        private void UpdateRotation()
        {
            if (!pingPong)
                angle += Time.deltaTime * rotateSpeed * (reversed ? -1 : 1);
            else
                angle = rotateAngle * Mathf.Sin((Time.time + _timeOffset) * rotateSpeed) / 2;

            var position = _orbitCenter - Quaternion.AngleAxis(angle, Vector3.forward) * orbitDirection * orbitDistance;
            transform.position = position;
            if (lookAtOrbit)
                transform.up = _orbitCenter - transform.position;
        }

        private void UpdatePosition()
        {
            var movAxis = movementSpace == Space.World ? movementAxis : transform.TransformDirection(movementAxis);

            var p1 = _initialPosition + movAxis * movementDistance / 2;
            var p2 = _initialPosition - movAxis * movementDistance / 2;

            movementRatio = (Mathf.Sin((_timeOffset + Time.time) * movementSpeed) + 1) / 2;
            transform.position = Vector3.Lerp(p1, p2, movementRatio);
        }
    }
}