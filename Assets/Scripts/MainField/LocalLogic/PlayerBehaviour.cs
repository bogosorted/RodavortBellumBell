using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HandBoardBehaviour board;
    
    bool isMultiplayer;
    int life;
    int gold;

    public void TryPutCardOnBoard(Card card)
    {
            
        //if aceppt
            RectTransform rectCard = card.transform.parent as RectTransform;
            RectTransform rectBoard = board.transform as RectTransform;
            CardAnimation cardAnim = card.GetComponent<CardAnimation>();

            //set the card to be a child of "CardsOutside", to be under of the real 
            //cards hand without automatically set the childrens by "HandBehavior"
            Transform playerHand = rectBoard.transform.parent.GetChild(1);
            rectCard.transform.SetParent(playerHand.GetChild(0));


            StartCoroutine(cardAnim.Dissolve(false));

            board.CreateCard(card);

            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition =  rectBoard.anchoredPosition + board.CalculeCardFinalPosition(board.GetHandCount) * Vector2.right ;

            card.startAngle = rectCard.rotation;
            card.finalAngle = Quaternion.identity;
             
            card.MoveTo(1,0,0);
            card.ChangeSize(0.4485f,2);

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
