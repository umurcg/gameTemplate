
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class RankGuid : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private new string name;
#endif

    public string Guid => guid;
    [SerializeField] protected string guid;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateGuid();
    }

    private void UpdateGuid()
    {
        var ranks = FindObjectsOfType<Rank>();
        foreach (Rank rank in ranks)
        {
            if (rank.name == name)
            {
                guid = rank.guid;
                EditorUtility.SetDirty(this);
                break;
            }
        }
    }

    [ContextMenu("Update GUID")]
    public void UpdateGuidContextMenu()
    {
        UpdateGuid();
    }
#endif
}

