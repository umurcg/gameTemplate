using UnityEngine;

namespace Helpers
{
    public class FingerSimulator : MonoBehaviour
    {
        [Header("Follow Parameters")]
        public float FollowSpeed = 10;
        public KeyCode FollowKey = KeyCode.LeftShift;

        [Header("Scale Parameters")]
        public float ActiveScale = 0.5f;
        public float ScaleDuration = 0.1f;
        public KeyCode ScaleKey = KeyCode.LeftControl;
        private bool fingerScaled;


        private Vector3 _originalScale;
        private RectTransform _rectTransform;
        
        private Vector3 aimScale;
        private float scaleSpeed;

        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
            aimScale = _originalScale;
            
            //Calculate scale speed from original scale and active scale
            scaleSpeed = (ActiveScale - _originalScale.x) / ScaleDuration; 

        }

        public void Update()
        {
            _rectTransform.transform.localScale = Vector3.MoveTowards(_rectTransform.transform.localScale, aimScale, Time.deltaTime * scaleSpeed);
            
            if (Input.GetKey(FollowKey))
            {
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, Input.mousePosition,Time.deltaTime*FollowSpeed);
            }

            if (!fingerScaled && Input.GetKeyDown(ScaleKey))
            {
                fingerScaled = true;
                aimScale = ActiveScale * Vector3.one;
            }else if (fingerScaled && Input.GetKeyUp(ScaleKey))
            {
             
                aimScale = _originalScale;
                fingerScaled = false;
            }
        }
    }
}