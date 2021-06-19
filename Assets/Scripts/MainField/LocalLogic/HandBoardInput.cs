using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandBoardInput : MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler
  
{
    [SerializeField] HandBehaviour playerHand;

      public void OnPointerEnter(PointerEventData eventData)
    {
        playerHand.ShowAmplifiedHand();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       playerHand.StopShowingAmplifiedHand();
    }


}
