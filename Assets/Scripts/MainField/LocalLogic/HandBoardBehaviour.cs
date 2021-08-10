using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoardBehaviour : MonoBehaviour
{
    List<Card> handBoard = new List<Card>();

    [Header("Board Refferences")]
    [SerializeField] HandBehaviour handBehave;
    [SerializeField] GameObject cardPrefab;

    [Header("Board Settings")]

    [SerializeField] float boardMaxWidth;
    [SerializeField] float maxHandAngle;
    [SerializeField] float handWidthMultiplier;
    [SerializeField] float handSizeIncreaseValue;

    [Header("Board Offset")]
    [SerializeField] Vector2 handOffset;
  
    float handXAxisWidth;

    //const int MaxCardInHand = 10; i dont finish this yet

    HandBehaviour.HandAnimationSettings boardAnimationSettings;
    Coroutine organizeHandCurrentCoroutine;

    
    void Awake() 
    {
        boardMaxWidth /= handWidthMultiplier;

        boardAnimationSettings = new HandBehaviour.HandAnimationSettings(handOffset,maxHandAngle,handWidthMultiplier,handXAxisWidth,true);
    }
    public int GetHandCount{get => handBoard.Count;}


    public void CreateCard(Card cardInfo)
    {

        GameObject newCard = Instantiate(cardPrefab,this.transform);
        Card cardAttributes = newCard.transform.GetChild(0).GetComponent<Card>();
        CardAnimation cardAnim = cardAttributes.transform.GetComponent<CardAnimation>();

        cardAttributes.ReceiveStartInfo(cardInfo.initialInfo);

        handBoard.Add(cardAttributes);

        float boardWidth = (handBoard.Count-1) * handSizeIncreaseValue;
        boardAnimationSettings.HandXAxisWidth = Mathf.Clamp(boardWidth,-boardMaxWidth/2,boardMaxWidth/2); 

        StartCoroutine(cardAnim.Dissolve(true));  
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(handBehave.OrganizeHand(boardAnimationSettings,handBoard));

    }
    public Vector2 CalculeCardFinalPosition(int handCount)
    {    
        float widthIncreaseConst = boardAnimationSettings.HandXAxisWidth / (handCount-1);
        float concat = -boardAnimationSettings.HandXAxisWidth;

        concat += concat + (handCount != 1 ? widthIncreaseConst * handCount * 2: 0);
        return  new Vector2((handCount != 1 ? boardAnimationSettings.HandXAxisWidth * boardAnimationSettings.WidthMultiplier: 0) ,boardAnimationSettings.OffSet.y + (-Mathf.Abs(concat)/(boardAnimationSettings.Angulation*15)));

    }
}
