using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandInput : MonoBehaviour ,IPointerExitHandler, IPointerEnterHandler  
{

  [SerializeField] HandBehaviour playerHand;

  [Header("Collider Attributes")]
  [SerializeField] Vector2 normalColliderSize;
  [SerializeField] Vector2 OnPointerColliderSize;

  [HideInInspector] public bool pointerOnBoard;

  RectTransform rect;

  void Awake()
  {
    rect = transform as RectTransform;
    ChangeColliderSize(normalColliderSize);
  }


  public void OnPointerEnter(PointerEventData eventData)
  {
    pointerOnBoard = true;

    ChangeColliderSize(OnPointerColliderSize);

    playerHand.ShowAmplifiedHand();
    playerHand.SetCardsHandRaycast(pointerOnBoard);

  }
  public void OnPointerExit(PointerEventData eventData)
  {
    pointerOnBoard = false;

    ChangeColliderSize(normalColliderSize);

    playerHand.SetCardsHandRaycast(pointerOnBoard);
    playerHand.StopShowingAmplifiedHand();
   
  }
  public void SetHandRaycast(bool statement) => GetComponent<Image>().raycastTarget = statement;

  private void ChangeColliderSize(Vector2 size) => rect.sizeDelta = size;
}
