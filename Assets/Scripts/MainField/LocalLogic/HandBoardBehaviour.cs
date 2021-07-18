using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoardBehaviour : MonoBehaviour
{
    [SerializeField] HandBehaviour handBehave;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] Vector2 handOffset;
    [SerializeField] float handWidthMultiplier;
    [SerializeField] float maxHandAngle;
    [SerializeField] float handSizeIncreaseValue;

    float handXAxisWidth;

    //const int MaxCardInHand = 10;

    List<Card> handBoard = new List<Card>();

    HandBehaviour.HandAnimationSettings boardAnimation;
    Coroutine organizeHandCurrentCoroutine;
    
    void Awake()
    {
        boardAnimation = new HandBehaviour.HandAnimationSettings(handOffset,maxHandAngle,handWidthMultiplier,handXAxisWidth,true);
    }

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
}
