
using Core.Enums;
using UnityEngine;

namespace Helpers
{
    public class TargetFollower : MonoBehaviour
    {
        public Transform Aim => target;
        
        [SerializeField] private Transform target;
        [SerializeField] private bool followPosition;
        [SerializeField] private bool preserveOffset;
        [SerializeField] private bool followPositionWithLerp;
        [SerializeField] private bool followPosX = true;
        [SerializeField] private bool followPosY = true;
        [SerializeField] private bool followPosZ = true;
        
        private Vector3 followPositionOffset;
        [SerializeField] private float followPositionLerpSpeed;

        [SerializeField] private bool followRotation;
        [SerializeField] private bool followRotationWithLerp;
        [SerializeField] private float followRotationLerpSpeed;

        [SerializeField] private UpdateMethod updateMethod;

        private void Awake()
        {
            followPositionOffset = target ? transform.position - target.position : Vector3.zero;
        }

        private void Update()
        {
            if(updateMethod == UpdateMethod.Update)
                Follow(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if(updateMethod == UpdateMethod.FixedUpdate)
                Follow(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            if(updateMethod == UpdateMethod.LateUpdate)
                Follow(Time.deltaTime);
        }

        private void Follow(float deltaTime)
        {
            if (!target)
                return;

            if (followPosition)
            {
                var aimPos = transform.position;
                if (followPosX) aimPos.x = target.position.x;
                if (followPosY) aimPos.y = target.position.y;
                if (followPosZ) aimPos.z = target.position.z;

                if (preserveOffset)
                {
                    if (followPosX) aimPos.x += followPositionOffset.x;
                    if (followPosY) aimPos.y += followPositionOffset.y;
                    if (followPosZ) aimPos.z += followPositionOffset.z;
                }

                if (followPositionWithLerp)
                    transform.position = Vector3.Lerp(transform.position, aimPos, deltaTime * followPositionLerpSpeed);
                else
                    transform.position = aimPos;
            }

            if (followRotation)
            {
                var aimRot = target.rotation;
                if (followRotationWithLerp)
                    transform.rotation = Quaternion.Lerp(transform.rotation, aimRot, deltaTime * followRotationLerpSpeed);
                else
                    transform.rotation = aimRot;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Fix To Target")]
        private void FixToTarget()
        {
            if (!target) return;
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
#endif
    }
}
