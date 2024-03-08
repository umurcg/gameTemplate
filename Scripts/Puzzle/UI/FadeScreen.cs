using System;
using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle.UI
{
    public class FadeScreen : Singleton<FadeScreen>
    {
        [SerializeField] private bool fadeOutOnGameStart;
        [SerializeField] private float maximumAlpha = 1;
        [SerializeField] private AnimationCurve fadeCurve;
        private Image _image;
        public bool IsFading { get; private set; }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.raycastTarget = false;
            SetAlpha(0);
        
        }

        private void Start()
        {
            if (fadeOutOnGameStart)
            {
                SetAlpha(1);
                FadeOut();
            }
        }

        public void SetAlpha(float alpha)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
        }
    
        public float GetAlpha()
        {
            return _image.color.a;
        }

    
        public void FadeIn(Action onComplete=null)
        {
            if (IsFading)
            {
                StopAllCoroutines();
                IsFading = false;
            }
        
            SetAlpha(0);
            StartCoroutine(Fade(maximumAlpha, 1, onComplete));
        }
    
        public void FadeOut(Action onComplete=null)
        {
            if (IsFading)
            {
                StopAllCoroutines();
                IsFading = false;
            }
        
            StartCoroutine(Fade(0, 1, onComplete));
        }

        public IEnumerator Fade(float aimAlpha, float duration, Action onComplete)
        {
            if (IsFading)
            {
                yield break;
            }
        
            float startAlpha = GetAlpha();
        
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                SetAlpha(Mathf.Lerp(startAlpha, aimAlpha, fadeCurve.Evaluate(t)));
                yield return null;
            }
            onComplete?.Invoke();
        }

    
    }
}
