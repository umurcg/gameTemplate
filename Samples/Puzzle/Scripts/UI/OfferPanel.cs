using System.Collections;
using Managers;
using PuzzleUI;
using Samples.Puzzle;
using Samples.Puzzle.Scripts;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class OfferPanel : UIPanel
{
    [SerializeField] public TextController[] moveCountTexts;
    [SerializeField] public TMPro.TMP_Text costText;

    
    private void Start()
    {
        foreach (var text in moveCountTexts)
        {
            text.SetText(PuzzleManager.Instance.AdditionalMoveCount.ToString());
        }

        costText.text = PuzzleManager.Instance.AdditionalMoveCost.ToString();
        PuzzleActions.Instance.OnAdditionalMoveOffer += Activate;
        PuzzleActions.Instance.OnAdditionalMoveBought += Deactivate;
        ActionManager.Instance.OnGameLost += Deactivate;
    }

    public void BuyMoves()
    {
        if (PuzzleManager.Instance.CanBuyMoves())
        {
            SetAnimationTrigger();
            PuzzleManager.Instance.BuyAdditionalMoves();
        }
        else
        {
            Debug.Log("Not enough currency");
        }
    }

    public void DeclineMoves()
    {
        SetAnimationTrigger();
        CoreManager.Instance.LostGame();
    }
    
}