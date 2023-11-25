
using ActionTriggers;
using UnityEngine;

namespace Core.ActionTriggers
{
    public class FeelTrigger : BaseTrigger
    {
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
        [SerializeField] private MoreMountains.Feedbacks.MMFeedbacks[] feedbacks;

        protected override void Trigger()
        {
            foreach (MoreMountains.Feedbacks.MMFeedbacks feedback in feedbacks)
            {
                feedback.PlayFeedbacks();
            }
        }
#else
        protected override void Trigger()
        {
            Debug.LogWarning("FeelTrigger is not working without NiceVibrations");
        }
#endif
    }
}
