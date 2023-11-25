
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonArea : MonoBehaviour
{
    public UnityEvent<float> timerUpdated;
    public UnityEvent<bool> buttonActivated;
    public bool IsActivated { get; private set; }
    
    [SerializeField] private float timeToActivate = 1f;
    
    private float _timer = 0f;
    private bool _isInside;
    private bool _isActive;

    private void Start()
    {
        timerUpdated?.Invoke(0f);
    }

    private void Update()
    {
        if (_isInside && !IsActivated)
        {
            _timer += Time.deltaTime;
            timerUpdated?.Invoke(_timer / timeToActivate);
            if (_timer >= timeToActivate)
            {
                _timer = timeToActivate;
                IsActivated = true;
                buttonActivated?.Invoke(true);
            }
            
        }
        else if (!_isInside && _timer > 0f)
        {
            if (IsActivated)
            {
                IsActivated = false;
                buttonActivated?.Invoke(false);
            }
            
            _timer -= Time.deltaTime * 5f;
            if (_timer < 0f) _timer = 0f;
            timerUpdated?.Invoke(_timer / timeToActivate);
        }
    }

    public void ResetTimer()
    {
        _timer = 0f;
        timerUpdated?.Invoke(0f);
        IsActivated = false;
        _isInside = false;
        buttonActivated?.Invoke(false);
    }

    public void PlayerInside()
    {
        _isInside = true;
    }
    
    public void PlayerOutside()
    {
        _isInside = false;
    }
}
