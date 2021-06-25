using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardInput :  MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] Card actualCard;

    HandBoardInput handBoard;
    HandBehaviour playerHand;

    float handlingCardSize = 0.7f;
    

   public void OnBeginDrag(PointerEventData eventData)
    {
        playerHand.SetCardsHandRaycast(false);
        handBoard.SetHandRaycast(false);
        GetComponent<Image>().raycastTarget = false;
        
        transform.SetAsLastSibling();
        transform.GetChild(0).rotation = Quaternion.identity;

        actualCard.ChangeSize(handlingCardSize, 2f);
        playerHand.RemoveCard(actualCard.posInHand);   
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        playerHand.AddCard(actualCard.posInHand,actualCard);
        transform.SetSiblingIndex(actualCard.posInHand);
        handBoard.SetHandRaycast(true);
       
    }

    void Awake()
    {
        playerHand = GameObject.Find("PlayerHand").GetComponent<HandBehaviour>();
        handBoard = playerHand.transform.GetComponent<HandBoardInput>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {   
   
        actualCard.ChangeSize(playerHand.showingHandSize + 0.1f,10);
        transform.SetAsLastSibling();
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {  
        if(!eventData.dragging)
        {
            transform.SetSiblingIndex(actualCard.posInHand);
            actualCard.ChangeSize(playerHand.showingHandSize,10);
            if(!handBoard.pointerOnBoard)
                playerHand.StopShowingAmplifiedHand();
        }
    }
}
