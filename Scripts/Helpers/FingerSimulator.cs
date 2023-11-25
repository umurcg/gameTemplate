// using DG.Tweening;
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
        // public Ease ScaleEase = Ease.OutQuad;
        private bool fingerScaled;


        private Vector3 _originalScale;
        private RectTransform _rectTransform;
    
        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        
        }

        public void Update()
        {
            if (Input.GetKey(FollowKey))
            {
                _rectTransform.position = Vector2.Lerp(_rectTransform.position, Input.mousePosition,Time.deltaTime*FollowSpeed);
            }

            // if (!fingerScaled && Input.GetKeyDown(ScaleKey))
            // {
            //     DOTween.Kill(_rectTransform);
            //     fingerScaled = true;
            //     _rectTransform.DOScale(ActiveScale * Vector3.one, ScaleDuration).SetEase(ScaleEase);
            // }else if (fingerScaled && Input.GetKeyUp(ScaleKey))
            // {
            //     DOTween.Kill(_rectTransform);
            //     _rectTransform.DOScale(_originalScale, ScaleDuration).SetEase(ScaleEase);
            //     fingerScaled = false;
            // }
        }
    

    }
}
