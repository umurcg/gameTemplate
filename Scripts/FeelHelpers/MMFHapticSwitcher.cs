using Helpers;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
using Lofelt.NiceVibrations;
#endif


public class MMFHapticSwitcher : SwitchButton
{
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
    protected new void Start()
    {
        base.Start();
        OnSwitch.AddListener(OnSwitched);
    }

    private void OnSwitched(bool isOn)
    {
        HapticController.hapticsEnabled = isOn;
    }
#endif
}