using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HandBehaviour hand; 
    [SerializeField] HandBoardBehaviour board;

    
    bool isMultiplayer;
    int life;
    int gold;

    public void TryPutCardOnBoard(Card card)
    {
        //if server receive
        if(true)
        {
            RectTransform rectCard = card.transform.parent as RectTransform;
            RectTransform rectBoard = board.transform as RectTransform;
            CardAnimation cardAnim = card.GetComponent<CardAnimation>();

            //set the card to be a child of "CardsOutside", to be under of the real 
            //cards hand without automatically set the childrens by "HandBehavior"
            rectCard.transform.SetParent(hand.transform.GetChild(0));
    
            StartCoroutine(cardAnim.Dissolve(false));

            hand.handActualCount--;
            board.CreateCard(card);
            
            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition =  rectBoard.anchoredPosition + board.CalculeCardFinalPosition(board.GetHandCount);

            card.startAngle = rectCard.rotation;
            card.finalAngle = Quaternion.identity;
             
            card.MoveTo(1,0,0);
            card.ChangeSize(0.4885f,2);

            
        }

    }
    void TryAttackCard()
    {

    }
    void TryAttackPlayer()
    {

    }
    void TryNextTurn()
    {
        
    }

}
