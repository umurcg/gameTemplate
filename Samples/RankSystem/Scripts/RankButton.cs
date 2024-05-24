
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Managers;
using Core.UI;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class RankButton : RankGuid
{
    public enum MoneyType
    {
        GameMoney,LevelMoney
    }
    
    public MoneyType moneyType = MoneyType.GameMoney;
    
    [SerializeField] private TextController levelField;
    [SerializeField] private MoneyText moneyField;
    [SerializeField] private Button _button;
    private Rank _rank;
    public UnityEvent onButtonUpdated;
    public UnityEvent onLevelMaxed;
    

    private float CurrentMoney => moneyType == MoneyType.GameMoney
        ? CoreManager.Instance.GameMoney
        : GameManager.Instance.LevelMoney; 
        
    void Start()
    {
        if (!_button) _button = GetComponentInChildren<Button>();
        if (_button == null)
        {
            Debug.LogError("No button component found!");
            return;
        }
        
        _rank = Rank.GetRank(guid);
        if (!_rank)
        {
            Debug.LogError("Rank not found");
            return;
        }

        _rank.OnLevelChanged += RankLevelChanged;
        _button.onClick.AddListener(ButtonClicked);
        UpdateButton();
        
        if(moneyType == MoneyType.GameMoney)
            GlobalActions.OnGameMoneyChanged += MoneyChanged;
        else
            GlobalActions.OnLevelMoneyChanged += MoneyChanged;
    }
    

    public void ButtonClicked()
    {
        if (moneyType == MoneyType.GameMoney)
            CoreManager.Instance.SpendMoney(_rank.GetNextLevelPrice());
        else
            GameManager.Instance.LevelMoney -= _rank.GetNextLevelPrice();
        
        _rank.BuyNextLevel();
    }
    
    private void MoneyChanged(float money)
    {
        UpdateButton();
    }
    
    private void RankLevelChanged(int arg0)
    {
        UpdateButton();
    }
    
    public void UpdateButton()
    {
        if(levelField) levelField.SetText(_rank.Level.ToString());
        if(moneyField) moneyField.SetText(_rank.GetNextLevelPrice());
        onButtonUpdated?.Invoke();

        bool interactable = _rank.CanAffordNextLevel(CurrentMoney);
        _button.interactable = interactable;
        
        if(_rank.IsMaxLevel)
            onLevelMaxed?.Invoke();
    }
}

