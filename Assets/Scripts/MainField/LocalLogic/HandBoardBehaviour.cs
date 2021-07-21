using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoardBehaviour : MonoBehaviour
{
    List<Card> handBoard = new List<Card>();

    [SerializeField] HandBehaviour handBehave;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] Vector2 handOffset;
    [SerializeField] float handWidthMultiplier;
    [SerializeField] float maxHandAngle;
    [SerializeField] float handSizeIncreaseValue;
  
    float handXAxisWidth;

    //const int MaxCardInHand = 10; i dont finish this yet

    HandBehaviour.HandAnimationSettings boardAnimation;
    Coroutine organizeHandCurrentCoroutine;

    
    void Awake()
    {
        boardAnimation = new HandBehaviour.HandAnimationSettings(handOffset,maxHandAngle,handWidthMultiplier,handXAxisWidth,true);
    }
    public int GetHandCount{get => handBoard.Count;}


    public void CreateCard(Card cardInfo)
    {
        GameObject newCard = Instantiate(cardPrefab,this.transform);
        Card cardAttributes = newCard.transform.GetChild(0).GetComponent<Card>();
        CardAnimation cardAnim = cardAttributes.transform.GetComponent<CardAnimation>();

        cardAttributes.ReceiveStartInfo(cardInfo.initialInfo);

        handBoard.Add(cardAttributes);
        boardAnimation.HandXAxisWidth = (handBoard.Count-1) * handSizeIncreaseValue;

        StartCoroutine(cardAnim.Dissolve(true));  
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(handBehave.OrganizeHand(boardAnimation,handBoard));
    }
    public Vector2 CalculeCardFinalPosition(int handCount)
    {
        float WidthConst = boardAnimation.HandXAxisWidth/ (handCount-1);
        float concat = -boardAnimation.HandXAxisWidth;

        concat += handCount != 1 ? WidthConst * 2 * handCount : 0;
        
        return  new Vector2((handCount != 1 ? (concat * boardAnimation.WidthMultiplier) + boardAnimation.OffSet.x - 50 : 0) ,boardAnimation.OffSet.y + (-Mathf.Abs(concat)/(boardAnimation.Angulation*15)));
    }
}
