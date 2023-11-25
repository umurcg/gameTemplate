#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
// using Lofelt.NiceVibrations;
#endif
using Managers;
using UnityEngine;

namespace ActionTriggers
{
    public class HapticTrigger : BaseTrigger
    {
// #if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
//         private HapticManager _hapticManager;
//         public HapticPatterns.PresetType presetType;
//
//         public new void Start()
//         {
//             base.Start();
//             _hapticManager = HapticManager.Request();
//         }
//
//         protected override void Trigger()
//         {
//             _hapticManager.Haptic(presetType);
//         }
//
// #else
        protected override void Trigger()
        {
            Debug.LogWarning("HapticTrigger is not working without NiceVibrations");
        }
// #endif
    }
}