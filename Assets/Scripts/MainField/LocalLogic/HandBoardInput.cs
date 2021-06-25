using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandBoardInput : MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler  
{

  [SerializeField] HandBehaviour playerHand;
  RectTransform rect;
  public bool pointerOnBoard;

  void Awake()=> rect = transform as RectTransform;

  public void OnPointerEnter(PointerEventData eventData)
  {
    pointerOnBoard = true;
    rect.sizeDelta = new Vector2(540,375);
    playerHand.ShowAmplifiedHand();
    playerHand.SetCardsHandRaycast(true);
  }
  public void OnPointerExit(PointerEventData eventData)
  {
    pointerOnBoard = false;
    rect.sizeDelta = new Vector2(540,200);
    playerHand.SetCardsHandRaycast(false);
    playerHand.StopShowingAmplifiedHand();
   
  }
  public void SetHandRaycast(bool statement)
  {
    GetComponent<Image>().raycastTarget = statement;
  }

  
}
