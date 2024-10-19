using CorePublic.CustomTypes;
using UnityEngine;

namespace Samples.StickmanPack
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private float speedFactor = 1f;
        [SerializeField] private string key = "speed";
        [SerializeField] private Vector3Boolean movementAxis = new Vector3Boolean(true, false, true);
        private Vector3 _lastPosition;
        private Animator _animator;
    
    
        // Start is called before the first frame update
        void Start()
        {
            _lastPosition = transform.position;
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var movement = transform.position - _lastPosition;
            movement= new Vector3(movementAxis.x?movement.x:0, movementAxis.y?movement.y:0, movementAxis.z?movement.z:0);
            var speed = movement.magnitude / Time.deltaTime;
            _animator.SetFloat(key, speed * speedFactor);
            _lastPosition = transform.position;
        
        }
    }
}
