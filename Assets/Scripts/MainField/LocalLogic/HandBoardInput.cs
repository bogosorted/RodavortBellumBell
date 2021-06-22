using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandBoardInput : MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler  
{

  [SerializeField] HandBehaviour playerHand;
  public bool active;
  public void OnPointerEnter(PointerEventData eventData)
  {
    playerHand.ShowAmplifiedHand();
    playerHand.SetHandRaycast(true);
    active = true;
  }
  public void OnPointerExit(PointerEventData eventData)
  {
    playerHand.StopShowingAmplifiedHand();
    playerHand.SetHandRaycast(false);
    active = false;
  }

}
