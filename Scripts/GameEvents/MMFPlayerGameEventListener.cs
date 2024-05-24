using GameEvents;
#if MOREMOUNTAINS_FEEDBACKS
using MoreMountains.Feedbacks;
#endif
using UnityEngine;

#if MOREMOUNTAINS_FEEDBACKS
[RequireComponent(typeof(MMF_Player))]
#endif
public class MMFPlayerGameEventListener: MonoBehaviour, IGameEventListener
{
    public bool setDirection = false;
    public bool reversedDirection = false;
    public GameEvent gameEvent;
    
    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    
    public void OnEventRaise()
    {
        #if MOREMOUNTAINS_FEEDBACKS
        var player = GetComponent<MMF_Player>();
        if (setDirection)
        {
            if (!reversedDirection)
            {
                player.SetDirectionTopToBottom();
            }else
            {
                player.SetDirectionBottomToTop();
            }
        }
        
        player.PlayFeedbacks();
        #endif
      
    }
    
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
    
    private void OnDestroy()
    {
        if (gameEvent != null && enabled) 
            gameEvent.UnregisterListener(this);
    }
    
    
}