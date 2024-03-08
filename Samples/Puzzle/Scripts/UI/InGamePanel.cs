using System;
using Managers;
using UnityEngine;

namespace PuzzleUI
{
    public class InGamePanel: UIPanel
    {
        public void Start()
        {
            ActionManager.Instance.OnGameStarted += Activate;
            ActionManager.Instance.OnLevelDestroyed += Deactivate;
        }
    }
}