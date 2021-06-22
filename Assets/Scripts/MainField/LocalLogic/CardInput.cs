using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInput :  MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] Card actualCard;
    HandBehaviour playerHand;
    HandBoardInput handBoardInput;
    int lasPos;
    

    void Awake()
    {
        playerHand = GameObject.Find("PlayerHand").GetComponent<HandBehaviour>();
        handBoardInput = playerHand.transform.GetComponent<HandBoardInput>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        actualCard.startSize = actualCard.transform.localScale;
        actualCard.ChangeSize(playerHand.showingHandSize + 0.1f,10);
        transform.SetAsLastSibling();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        actualCard.startSize =  actualCard.transform.localScale;
        actualCard.ChangeSize(playerHand.showingHandSize,10);
        transform.SetSiblingIndex(actualCard.posInHand);
        if(!handBoardInput.active)
            playerHand.StopShowingAmplifiedHand();
    }
}
