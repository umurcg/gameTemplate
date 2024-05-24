using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextLevelButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(NextLevel);
    }

    private void NextLevel()
    {
        CoreManager.Instance.IncreaseLevel();
    }

}
