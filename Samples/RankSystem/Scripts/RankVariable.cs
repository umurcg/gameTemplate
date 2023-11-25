
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RankVariable<T>: RankGuid
{

    [SerializeField] public UnityEvent<T> setterEvent;
    public T[] levelValues;
    public Action<T> VariableChanged;
    private Rank _rank;

    private void Start()
    {
        if (string.IsNullOrEmpty(guid))
        {
            Debug.LogError("GUID is empty");
            return;
        }

        _rank = Rank.GetRank(guid);

        setterEvent?.Invoke(GetLevelVariable(_rank.Level));
        _rank.OnLevelChanged += LevelChanged;
    }

    public T GetCurrentVariable()
    {
        return GetLevelVariable(_rank.Level);
    }

    public virtual T GetLevelVariable(int level)
    {
        if (level >= levelValues.Length)
        {
            Debug.LogError("Level out of range");
            return default;
        }
            
        return levelValues[level];

    }
        
    public void LevelChanged(int level)
    {
        setterEvent?.Invoke(GetLevelVariable(level));
        VariableChanged?.Invoke(GetLevelVariable(level));
    }

    // Additional methods and implementation details if necessary
        
}
