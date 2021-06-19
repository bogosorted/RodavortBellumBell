using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBehaviour : MonoBehaviour
{
    List<Card> hand = new List<Card>();
    List<Card> handOnBoard = new List<Card>();

    const int MaxCardInHand = 10;
    [SerializeField] GameObject cardPrefab;

    [Header("Initial Created Card Settings")]
    [SerializeField] float initCardAnimationSpeed;
    [SerializeField] float initCardShowTime,sizeInitShowCard,initCardYMaxCurvePos,initCardXMaxCurvePos;
    [SerializeField] Vector2 startPosInitialCard,finalPosInitialCard;
    
    [Header("Hand Card Settings")]
    [SerializeField] float handAnimationSpeed;  
    [SerializeField] float handXAxisWidth,MaxHandAngle,handCardSize,handSizeIncreaseValue,showingHandSize;
    [SerializeField] Vector2 handOffset,showingHandOffset;
    
    Coroutine handSizeCurrentCoroutine,organizeHandCurrentCoroutine;
  //  void Start() => ((RectTransform)transform).anchoredPosition += handOffset; 

    void Update()
    {
        //test will be removed on realese
        if(Input.GetButtonDown("Fire1"))
        {
            CreateCard(0);
        }   
        
    }

    void CreateCard(int index)
    { 
        CardsInfo cardInfo = Resources.Load<CardsInfo>("Db/DbCardsAttributes/" + index.ToString());

        GameObject refCard = Instantiate(cardPrefab,this.transform);
        Card newCard = refCard.transform.GetChild(0).GetComponent<Card>();
      
        newCard.ReceiveStartInfo(cardInfo);
        newCard.startPosition = startPosInitialCard;
        newCard.finalPosition = finalPosInitialCard;

        
        
        StartCoroutine(ShowInitializedCard(newCard));   
        // hand.Add(newCard);
        // StartCoroutine(OrganizeHand());
        
    }
   
    void SetCardsPosition(Vector2 offSet)
    {
        RectTransform rectCard;
        float WidthConst = handXAxisWidth/ (hand.Count - 1);
        float concat = -handXAxisWidth;

        foreach(Card card in hand)
        {          
            rectCard = card.transform.parent as RectTransform;

            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition = new Vector2((hand.Count != 1 ? concat * offSet.x : 0) + offSet.x, offSet.y);

            card.startSize = rectCard.localScale;

            //ROTATE ONLY THE IMAGE AND NOT THE GRAPHIC_COLLIDER
            rectCard = card.transform as RectTransform;

            card.startAngle = rectCard.transform.rotation;
            card.finalAngle = Quaternion.Euler(0,0,hand.Count > 2 ?(-concat/handXAxisWidth)* MaxHandAngle:0);

            concat += WidthConst * 2;   
        }
    }

     IEnumerator ShowInitializedCard(Card card)
    {
        RectTransform rectCard = card.transform.parent as RectTransform;
        Vector3 finalPositionWithCurve;
        float curvePosX,curvePosY;
        
        //x its constant, y its smooth. both are 1 when the another be 1
        float x,y;

        card.startAngle = Quaternion.Euler(0,90,90);
        card.finalAngle = Quaternion.identity;

        x = 0;
        while (x <= 1)
        {          
            x += (initCardAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;

            curvePosX = (y * - initCardXMaxCurvePos + card.finalPosition.x) + initCardXMaxCurvePos;
            curvePosY = (y * -initCardYMaxCurvePos + card.finalPosition.y) + initCardYMaxCurvePos;
            finalPositionWithCurve = new Vector3(curvePosX,curvePosY);

            rectCard.transform.rotation = Quaternion.Lerp(card.startAngle,card.finalAngle,y);
            rectCard.anchoredPosition = Vector3.Lerp(card.startPosition,finalPositionWithCurve,y);
            rectCard.localScale = Vector3.one * ((y+1)/2)* sizeInitShowCard;

            yield return null;
        }   
            
        yield return new WaitForSeconds(initCardShowTime);

        hand.Add(card);
        
        handXAxisWidth += (hand.Count-1) * handSizeIncreaseValue;
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        if (handSizeCurrentCoroutine != null){StopCoroutine(handSizeCurrentCoroutine);}

        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(handOffset));
        handSizeCurrentCoroutine = StartCoroutine(ChangeHandSize(handCardSize));
    }
    
    IEnumerator OrganizeHand(Vector2 offSet)
    {
        RectTransform rectCard;
        float x,y;
        x = 0;

        
        SetCardsPosition(offSet);

        while(x<=1)
        {
            x += (handAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
           
            foreach(Card card in hand)
            { 
                rectCard = card.transform.parent as RectTransform;
                rectCard.anchoredPosition = Vector3.Lerp(card.startPosition,card.finalPosition,y);

                // ROTATE ONLY THE CARD AND NOT THE GRPAHIC_COLLIDER
                rectCard = card.transform as RectTransform;
                rectCard.transform.rotation = Quaternion.Lerp(card.startAngle,card.finalAngle,y);              
            }
            yield return null;
        }
    }

    IEnumerator ChangeHandSize(float size)
    {
        RectTransform rectCard;
        float x,y;
        x = 0;
        while(x <= 1)
        { 
            x += (initCardAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
            
            foreach(Card card in hand)
            { 
                rectCard = card.transform.parent as RectTransform;
                rectCard.localScale = Vector3.one * Mathf.Lerp(card.startSize.x,size,y);
            }      
            yield return null;
        }       
    }
    public void ShowAmplifiedHand()
    {
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        if (handSizeCurrentCoroutine != null){StopCoroutine(handSizeCurrentCoroutine);}       
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(showingHandOffset));
        handSizeCurrentCoroutine = StartCoroutine(ChangeHandSize(showingHandSize));

    }
    public void StopShowingAmplifiedHand()
    {
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        if (handSizeCurrentCoroutine != null){StopCoroutine(handSizeCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(handOffset));
        handSizeCurrentCoroutine = StartCoroutine(ChangeHandSize(handCardSize));
    }

    // organizar cartas 
    // remover carta da mão
    // adicionar carta da mão
    // adicionar carta ao campo
    // remover carta do campo
  
}
