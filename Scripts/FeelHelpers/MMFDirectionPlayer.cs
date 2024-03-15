using System;
using System.Collections;
using System.Collections.Generic;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
using MoreMountains.Feedbacks;
#endif
using UnityEngine;


#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
[RequireComponent(typeof(MMF_Player))]
#endif

public class MMFDirectionPlayer : MonoBehaviour
{
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
    private MMF_Player _player;

    private void Awake()
    {
        _player = GetComponent<MMF_Player>();
    }

    public void PlayTopToBottom()
    {
        if (_player.IsPlaying) _player.StopFeedbacks();
        _player.SetDirectionTopToBottom();
        _player.PlayFeedbacks();
    }

    public void PlayBottomToTop()
    {
        if (_player.IsPlaying) _player.StopFeedbacks();
        _player.SetDirectionBottomToTop();
        _player.PlayFeedbacks();
    }

#endif
}