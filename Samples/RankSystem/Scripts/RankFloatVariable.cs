
using UnityEngine;

[AddComponentMenu("*Reboot/Rank/Rank Float Variable")]
public class RankFloatVariable : RankVariable<float>
{
    public float overflowPower = 1.15f;
    
    public override float GetLevelVariable(int level)
    {
        if (level < levelValues.Length)
            return levelValues[level];
        
        float initialPrice = levelValues.Length > 0 ? levelValues[^1] : 1;
        int power = level - levelValues.Length;
        if (levelValues.Length > 0)
            power++;
        
        return initialPrice * Mathf.Pow(overflowPower, power);
    }

#if UNITY_EDITOR
    [ContextMenu("Calculate Level Variable")]
    private void CalculateLevelVariable()
    {
        // This method will only be available from the Unity Editor context menu.
        // Replace level with the specific level for which you want to calculate the variable.
        int level = 0; // Example level, set this to the desired level to test with.
        Debug.Log(GetLevelVariable(level));
    }
#endif
}
