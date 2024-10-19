using System;
using System.Collections;
using System.Collections.Generic;
using CorePublic.Helpers;
using CorePublic.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(GUIDGenerator))]
public class MoneyArea : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unlockPriceText;
    [SerializeField] private float moneyPerSecond = 1;
    
    [SerializeField] private GUIDGenerator guidGenerator;

    public bool useInitialUnlockPrice;
    public int initialUnlockPrice = 100;

    private string GUID
    {
        get
        {
            if (guidGenerator == null)
            {
                guidGenerator = GetComponent<GUIDGenerator>();
            }
            return guidGenerator.GUID;
        }
    }
        
    
    
    private float UnlockPrice
    {
        get => PlayerPrefs.GetFloat(GUID+"_unlockPrice", -1);
        set => PlayerPrefs.SetFloat(GUID+"_unlockPrice", value);
    }
    
    private float InvestedMoney
    {
        get=>PlayerPrefs.GetFloat(GUID+"_invested",0);
        set=>PlayerPrefs.SetFloat(GUID+"_invested",value);
    }

    public bool IsUnlocked=> InvestedMoney >= UnlockPrice;
    public UnityEvent onUnlock;
    private bool _isActive;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        
        if (!HasUnlockPrice())
        {
            if (useInitialUnlockPrice)
            {
                UnlockPrice = initialUnlockPrice;
            }
            else
            {
                Debug.LogError("Unlock price not set for " + name);
                enabled = false;
                yield break;
            }
        }
        UpdateTextField();
        if (IsUnlocked) onUnlock?.Invoke();
    }
    
    public bool HasUnlockPrice()
    {
        return PlayerPrefs.HasKey(GUID+"_unlockPrice");
        
    }
    
    private void Update()
    {
        if (_isActive && CoreManager.Instance.IsGameStarted && !IsUnlocked)
        {
            float playerMoney=CoreManager.Instance.GameMoney;
            var moneyToInvest = Time.deltaTime * moneyPerSecond;
            if (playerMoney >= moneyToInvest)
            {
                CoreManager.Instance.SpendMoney(moneyToInvest);
                InvestedMoney += moneyToInvest;
                InvestedMoney = Mathf.Clamp(InvestedMoney, 0, UnlockPrice);
                if(IsUnlocked) onUnlock?.Invoke();
                UpdateTextField();
            }
        }
    }
    
    public void ResetPrice(float newUnlockPrice)
    {
        UnlockPrice = newUnlockPrice;
        InvestedMoney = 0;
        UpdateTextField();
    }
    
    private void UpdateTextField()
    {
        float remainingMoney = UnlockPrice - InvestedMoney;
        unlockPriceText.text = Mathf.RoundToInt(remainingMoney).ToString();
    }

    public void SetActive(bool activate)
    {
        _isActive = activate;
    }
    
    
    
}
