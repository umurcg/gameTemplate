using Managers;
using UI;
using UnityEngine;

namespace Puzzle.UI
{
    public class OfferPanel : UIPanel
    {
        [SerializeField] public TextController[] moveCountTexts;
        [SerializeField] public TMPro.TMP_Text costText;

    
        private void Start()
        {
            foreach (var text in moveCountTexts)
            {
                text.SetText(MoveManager.Instance.AdditionalMoveCount.ToString());
            }

            costText.text = MoveManager.Instance.AdditionalMoveCost.ToString();
            PuzzleActions.OnAdditionalMoveOffer += Activate;
            PuzzleActions.OnAdditionalMoveBought += Deactivate;
            GlobalActions.OnGameLost += Deactivate;
        }

        public void BuyMoves()
        {
            if (MoveManager.Instance.CanBuyMoves())
            {
                SetAnimationTrigger();
                MoveManager.Instance.BuyAdditionalMoves();
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
}