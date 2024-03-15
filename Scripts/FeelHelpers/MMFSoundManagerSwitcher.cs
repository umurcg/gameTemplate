using Helpers;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
using MoreMountains.Tools;
#endif


public class MMFSoundManagerSwitcher : SwitchButton
{
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
    protected new void Start()
    {
        base.Start();
        OnSwitch.AddListener(OnSwitched);
    }

    private void OnSwitched(bool isOn)
    {
        var mmfSoundManager = FindObjectOfType<MMSoundManager>();
        if (mmfSoundManager != null)
        {
            if (isOn)
            {
                mmfSoundManager.UnmuteMaster();
            }
            else
            {
                mmfSoundManager.MuteMaster();
            }
        }
    }
#endif
}