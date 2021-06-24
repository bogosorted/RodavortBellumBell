using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandBoardInput : MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler  
{

  [SerializeField] HandBehaviour playerHand;
  RectTransform rect;
  public bool active;

  void Awake()=> rect = transform as RectTransform;

  public void OnPointerEnter(PointerEventData eventData)
  {
    rect.sizeDelta = new Vector2(540,400);
    playerHand.ShowAmplifiedHand();
    playerHand.SetHandRaycast(true);
    active = true;
  }
  public void OnPointerExit(PointerEventData eventData)
  {
     rect.sizeDelta = new Vector2(540,200);
    playerHand.StopShowingAmplifiedHand();
    playerHand.SetHandRaycast(false);
    active = false;
  }

}
