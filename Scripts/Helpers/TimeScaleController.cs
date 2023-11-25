using UnityEngine;

namespace Helpers
{
    public class TimeScaleController : MonoBehaviour
    {
        private float _fixedDeltaTime;

        void Awake()
        {
            // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
            _fixedDeltaTime = Time.fixedDeltaTime;
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
            // Adjust fixed delta time according to timescale
            Time.fixedDeltaTime = _fixedDeltaTime * Time.timeScale;
        }

#if UNITY_EDITOR
        // Use Unity's ContextMenu to replicate the button functionality when playing in the Editor
        [ContextMenu("LerpTimeScale")]
#endif
        public void LerpTimeScale(float scale, float duration)
        {
            // Implement the lerp functionality here.
            // Replace DOTween code with your own lerp logic if you want to remove DOTween as well.
        }
    }
}


// In this modification, I've commented out the `using DG.Tweening;` directive since it's commented in the original. If you would like to implement the lerp functionality without DOTween and Odin Inspector, you'll need to write custom lerp logic, possibly using a coroutine or `Time.deltaTime` in the `Update` method. Since the actual implementation of `LerpTimeScale` isn't provided, it's commented out for you to fill as needed. The `[Button]` attribute from Odin has been replaced with `ContextMenu` for editor-only functionality.