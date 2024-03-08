using System.Collections;
using System.Collections.Generic;
using Managers;
using UI;
using UnityEngine;

public class LevelTextField : TextController
{
    public int offset=1;

    private void Start()
    {
        ActionManager.Instance.OnLevelChanged+=LevelIsChanged;
        LevelIsChanged(CoreManager.Instance.Level);
    }

    private void OnDestroy()
    {
        if(!ActionManager.Instance) return;
        ActionManager.Instance.OnLevelChanged-=LevelIsChanged;
    }

    private void LevelIsChanged(int level)
    {
        level+=offset;
        SetText(level.ToString());
    }
}
