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

        card.startPosition = rectCard.anchoredPosition;
        card.finalPosition = rectBoard.anchoredPosition;

        card.finalAngle = Quaternion.identity;


        card.MoveTo(1,0,0);

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
