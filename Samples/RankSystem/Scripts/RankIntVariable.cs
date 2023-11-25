using UnityEngine;

[AddComponentMenu("*Reboot/Rank/Rank Int Variable")]
public class RankIntVariable : RankVariable<int>
{
    public float overflowPower = 1.15f;

    public override int GetLevelVariable(int level)
    {
        if (level < levelValues.Length)
            return levelValues[level];

        float initialPrice = levelValues.Length > 0 ? levelValues[^1] : 1;
        int power = level - levelValues.Length;
        if (levelValues.Length > 0)
            power++;

        int result = (int)initialPrice;
        for (int i = 0; i < power; i++)
        {
            result += result * (int)overflowPower;
        }

        return result;
    }

#if UNITY_EDITOR
    [ContextMenu("Calculate Level Variable")]
    private void CalculateLevelVariable()
    {
        // This method is meant to mimic the button functionality from Odin Inspector
        // Replace 'someLevelValue' with the level value you want to test with
        int someLevelValue = 0;
        Debug.Log(GetLevelVariable(someLevelValue));
    }
#endif
}

