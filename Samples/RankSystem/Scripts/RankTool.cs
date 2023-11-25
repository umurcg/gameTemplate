
using System;
using UnityEngine;

[RequireComponent(typeof(Rank))]
public class RankTool : MonoBehaviour
{
    private Rank _rank;
    public int Level => _rank ? _rank.Level : -1;

    [ContextMenu("Set Rank")]
    public void SetRank(int rank)
    {
        _rank.Level = rank;
    }

    private void OnValidate()
    {
        if (_rank == null)
            _rank = GetComponent<Rank>();
    }

    [ContextMenu("Set Values With Multiplication")]
    private void SetValuesWithMultiplication(float baseValue, float multiplier, int count)
    {
        _rank.price.values = new float[count];
        for (int i = 0; i < count; i++)
        {
            _rank.price.values[i] = baseValue * Mathf.Pow(multiplier, i);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_rank);
#endif
    }

    [ContextMenu("Set Values With Addition")]
    private void SetValuesWithAddition(float baseValue, float addition, int count)
    {
        _rank.price.values = new float[count];
        for (int i = 0; i < count; i++)
        {
            _rank.price.values[i] = baseValue + addition * i;
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_rank);
#endif
    }
}
