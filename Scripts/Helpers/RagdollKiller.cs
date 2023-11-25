#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class RagdollKiller : MonoBehaviour
    {
        public Vector3 forcePerBone = new Vector3(0, 50, 0);
        private readonly Dictionary<Rigidbody, Vector3> _localPositions = new Dictionary<Rigidbody, Vector3>();
        private readonly Dictionary<Rigidbody, Quaternion> _localRotations = new Dictionary<Rigidbody, Quaternion>();
        private Rigidbody[] _bodies;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _bodies = GetComponentsInChildren<Rigidbody>();
            foreach (var body in _bodies)
            {
                body.isKinematic = true;
                _localPositions.Add(body, body.transform.localPosition);
                _localRotations.Add(body, body.transform.localRotation);
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Die()
        {
            if (_animator) _animator.enabled = false;
            foreach (var body in _bodies)
            {
                body.isKinematic = false;
                body.AddForce(forcePerBone);
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Revive()
        {
            foreach (var body in _bodies)
            {
                body.angularVelocity = Vector3.zero;
                body.velocity = Vector3.zero;
                body.isKinematic = true;
                var bodyTransform = body.transform;
                bodyTransform.localPosition = _localPositions[body];
                bodyTransform.localRotation = _localRotations[body];
            }
            if (_animator) _animator.enabled = true;
        }
    }
}
