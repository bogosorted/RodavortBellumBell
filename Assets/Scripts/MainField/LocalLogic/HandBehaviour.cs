using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBehaviour : MonoBehaviour
{
    List<Card> hand = new List<Card>();
    List<Card> handOnBoard = new List<Card>();


    [SerializeField] float initCardAnimationSpeed,initCardShowTime,sizeInitShowCard;
    [SerializeField] float handXAxisWidth,handAngle,handCardSize,handSizeIncreaseValue;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Vector3 startPosInitialCard,finalPosInitialCard;

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
        float x,y;
        
        x = 0;
        while (x <= 1)
        {          
            x += (initCardAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
        
            rectCard.anchoredPosition = Vector3.Lerp(card.startPosition,card.finalPosition,y);
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
            card.finalAngle =Quaternion.Euler(0,0,hand.Count != 1 ?(-concat/handXAxisWidth)* handAngle:0);

            concat += WidthConst * 2;   
        }

        while(x<=1)
        {
            x += (initCardAnimationSpeed * Time.deltaTime);
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
