using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBehaviour : MonoBehaviour
{
    List<Card> hand = new List<Card>();
    List<Card> handOnBoard = new List<Card>();

    [SerializeField] GameObject cardPrefab;

    [Header("Initial Created Card Settings")]
    [SerializeField] float initCardAnimationSpeed;
    [SerializeField] float initCardShowTime,sizeInitShowCard,initCardYMaxCurvePos,initCardXMaxCurvePos;
    [SerializeField] Vector3 startPosInitialCard,finalPosInitialCard;
    
    [Header("Hand Card Settings")]
    [SerializeField] float handAnimationSpeed;  
    [SerializeField] float handXAxisWidth,MaxHandAngle,handCardSize,handSizeIncreaseValue;
    

    void Start()
    {
    }
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
      
        InitializeCard(newCard,cardInfo);   
        
    }
    void InitializeCard(Card card,CardsInfo cardInfo)
    {
        
        card.ReceiveStartInfo(cardInfo);
        card.startPosition = startPosInitialCard;
        card.finalPosition = finalPosInitialCard;
        StartCoroutine(ShowInitializedCard(card));     
    }
   
    IEnumerator ShowInitializedCard(Card card)
    {
        RectTransform rectCard = card.transform.parent.GetComponent<RectTransform>();
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
        
        StartCoroutine(OrganizeHand());

        x = 0;
        while(x <= 1)
        { 
            x += (initCardAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
            
            rectCard.localScale = Vector3.one * Mathf.Lerp(sizeInitShowCard,handCardSize,y);
            yield return null;
        }       
    }

    IEnumerator OrganizeHand()
    {
        RectTransform rectCard;
        float x,y;
        x = 0;

        handXAxisWidth += (hand.Count-1) * handSizeIncreaseValue;

        float WidthConst = handXAxisWidth / (hand.Count - 1);
        float concat = -handXAxisWidth;

        foreach(Card card in hand)
        {          
            rectCard = card.transform.parent.GetComponent<RectTransform>();

            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition = new Vector3(hand.Count != 1 ? concat : 0, 0);

            card.startAngle = rectCard.transform.rotation;
            card.finalAngle =Quaternion.Euler(0,0,hand.Count > 2 ?(-concat/handXAxisWidth)* MaxHandAngle:0);

            concat += WidthConst * 2;   
        }

        while(x<=1)
        {
            x += (handAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
           
            foreach(Card card in hand)
            { 
                rectCard = card.transform.parent.GetComponent<RectTransform>();
                rectCard.anchoredPosition = Vector3.Lerp(card.startPosition,card.finalPosition,y);
                rectCard.transform.rotation = Quaternion.Lerp(card.startAngle,card.finalAngle,y);
            }
            yield return null;
        }
    }

    // organizar cartas 
    // remover carta da mão
    // adicionar carta da mão
    // adicionar carta ao campo
    // remover carta do campo
  
}
