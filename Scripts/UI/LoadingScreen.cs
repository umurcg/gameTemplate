// using DG.Tweening;

using CorePublic.Helpers;
using UnityEngine.UI;

namespace CorePublic.UI
{
    public class LoadingScreen : Singleton<LoadingScreen>
    {
        public float fadeDuration;

        private bool _fadedIn = true;
        private MaskableGraphic[] _graphics;

        // Start is called before the first frame update
        private void Start()
        {
            _graphics = GetComponentsInChildren<MaskableGraphic>();
            SetAlpha(1);
        }


        public void FadeIn()
        {
            if (_fadedIn)
                return;

            SetAlpha(0);
            // DOTween.ToAlpha(() => _graphics[0].color, x => SetAlpha(x.a), 1, fadeDuration);

            _fadedIn = true;
        }

        public void FadeOut()
        {
            if (!_fadedIn)
                return;

            SetAlpha(1);
            // DOTween.ToAlpha(() => _graphics[0].color, x => SetAlpha(x.a), 0, fadeDuration);

            _fadedIn = true;
        }

        private void SetAlpha(float a)
        {
            foreach (var m in _graphics)
            {
                var col = m.color;
                col.a = a;
                m.color = col;
            }
        }
    }
}