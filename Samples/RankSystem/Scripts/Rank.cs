
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("*Reboot/Rank/Rank")]
[Serializable]public class Rank : MonoBehaviour
{
    public static List<Rank> Ranks = new List<Rank>();
    [SerializeField] public string guid;
    public string name;
    public LevelDependentVariable price;
    
    
    public int Level
    {
        get => PlayerPrefs.GetInt(guid+"_level",0);
        set => PlayerPrefs.SetInt(guid+"_level",value);
    }

    public bool IsMaxLevel => price.IsMaxLevel(Level);
    
    public Action<int> OnLevelChanged;
    

    private void Awake()
    {
        if(Ranks==null) Ranks = new List<Rank>();
        Ranks.Add(this);
    }


    public float GetNextLevelPrice()
    {
        return price.CalculateVariable(Level);
    }
        
    public bool CanAffordNextLevel(float money)
    {
        if (price.IsMaxLevel(Level)) return false;
        return GetNextLevelPrice() <= money;
    }
    
    public void BuyNextLevel()
    {
        if (price.IsMaxLevel(Level))
        {
            Debug.LogError("Max level reached");
            return;
        }
        Level++;
        OnLevelChanged?.Invoke(Level);
    }

    [ContextMenu("LevelUp")]
    public void LevelUp()
    {
        Level++;
        OnLevelChanged?.Invoke(Level);
    }

    private void OnDestroy()
    {
        Ranks.Remove(this);
    }
    
    [ContextMenu("CalculateLevelPrice")]
    public float CalculateLevelPrice(int level)
    {
        return price.CalculateVariable(level);
    }
    
    public static Rank GetRank(string guid)
    {
        return Ranks.Find(rank => rank.guid == guid);
    }
    
#if UNITY_EDITOR
    [ContextMenu("RefreshGuid")]
    private void RefreshGuid()
    {
        guid = System.Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(guid))
        {
            RefreshGuid();
        }
    }
        
#endif
}
