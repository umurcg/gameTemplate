using System;
using UnityEngine;

[Serializable]
public class LevelDependentVariable
{
    [SerializeField] private float powerFactor = 1.15f;
    public float[] values;
    [SerializeField] private bool hasMaxLevel;
    // Having ShowIf attribute replaced by [HideInInspector] or similar logic in code
    [SerializeField] private int maxLevel = 99;

    public float CalculateVariable(int level)
    {
        if (hasMaxLevel && level > maxLevel)
        {
            Debug.LogError("Level is higher than max level");
            return -1;
        }

        if (level < values.Length)
            return values[level];

        float initialPrice = values.Length > 0 ? values[values.Length - 1] : 1;
        int power = level - values.Length;
        if (values.Length > 0)
            power++;

        return initialPrice * Mathf.Pow(powerFactor, power);
    }

    public bool IsMaxLevel(int level)
    {
        return hasMaxLevel && level >= maxLevel;
    }
}
