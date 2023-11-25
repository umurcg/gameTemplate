using Helpers;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
// using Lofelt.NiceVibrations;
#endif
using UnityEngine;

namespace Managers
{
    public class HapticManager : Singleton<HapticManager>
    {
//
// #if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
//         [SerializeField]private float minBreak;
//         private float _lastHapticTime = Mathf.NegativeInfinity;
//
//         public bool showHaptic;
//     
//
//         // Update is called once per frame
//         public void Haptic(HapticPatterns.PresetType presetType)
//         {
//             if(Time.time-_lastHapticTime<minBreak) return;
//             HapticPatterns.PlayPreset(presetType);
//             _lastHapticTime = Time.time;
//             
// #if UNITY_EDITOR
//             Blink();
// #endif
//             
//         }
//         
//         
//         
// #if UNITY_EDITOR
//         
//         public Texture pointTexture;
//         private int blinkCounter;
//
//         void Blink()
//         {
//             blinkCounter = 5;
//         }
//         
//         void OnGUI()
//         {
//             if (!showHaptic || !pointTexture || blinkCounter<=0) return;
//             float width = Screen.width / 20;
//             GUI.DrawTexture(new Rect(10, 10, width, width), pointTexture, ScaleMode.ScaleToFit, true, 1,Color.red,0,0);
//             blinkCounter--;
//         }
//         
// #endif
//         
// #endif
    }
}