using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBehaviour : MonoBehaviour
{
    List<Card> hand = new List<Card>();

    const int MaxCardInHand = 10;

    [SerializeField] GameObject cardPrefab;

    [Header("Initial Created Card Settings")]
    [SerializeField] float initCardAnimationSpeed;
    [SerializeField] float initCardShowTime,sizeInitShowCard,initCardYMaxCurvePos,initCardXMaxCurvePos,maxShowingAngle;
    [SerializeField] Vector2 startPosInitialCard,finalPosInitialCard;
    [SerializeField] float showingHandWidthMultiplier;

    
    [Header("Hand Card Settings")]
    [SerializeField] float handAnimationSpeed;  
    [SerializeField] float handXAxisWidth,maxHandAngle,handSizeIncreaseValue;
    [SerializeField] Vector2 handOffset,showingHandOffset;
    [SerializeField] float handWidthMultiplier;

    [Header("Public Variables")]
    public float showingHandSize,handCardSize;
    
    HandAnimationSettings playerHandAnimation,showingHandAnimation;
    Coroutine organizeHandCurrentCoroutine;

    //the card only its added on final animation of initialized
    // that is the accurate hand.count
    int handActualCount;

    public struct HandAnimationSettings
    {
        public HandAnimationSettings(Vector2 offSet,float angulation,float widthMultiplier,float handXWidth,bool playerHand)
        {
            OffSet = offSet;
            Angulation = angulation;
            WidthMultiplier = widthMultiplier;
            HandXAxisWidth = handXWidth;
            PlayerHand = playerHand;
        }

        public Vector2 OffSet{get;}
        public float Angulation{get;}
        public float WidthMultiplier{get;}
        public float HandXAxisWidth{get;set;}
        public bool PlayerHand{get;}
    }


    void Awake()
    {
        playerHandAnimation = new HandAnimationSettings(handOffset,maxHandAngle,handWidthMultiplier,handXAxisWidth,true);
        showingHandAnimation = new HandAnimationSettings(showingHandOffset,maxShowingAngle,showingHandWidthMultiplier,handXAxisWidth,true);
        CreateCard(0);CreateCard(0);CreateCard(0);CreateCard(0);
    }
    void Update()
    {
     //   test will be removed on realese
        if(Input.GetKeyDown(KeyCode.Z))
        {
            CreateCard(0);
        }   
        
    }

    void CreateCard(int index)
    { 
        if(handActualCount < MaxCardInHand)
        {

            handActualCount++;

            CardsInfo cardInfo = Resources.Load<CardsInfo>("Db/DbCardsAttributes/" + index.ToString());

            GameObject refCard = Instantiate(cardPrefab,this.transform);
            Card newCard = refCard.transform.GetChild(0).GetComponent<Card>();
        
            newCard.ReceiveStartInfo(cardInfo);
            
            newCard.startPosition = startPosInitialCard;
            newCard.finalPosition = finalPosInitialCard;
        
            StartCoroutine(ShowInitializedCard(newCard));
            
        }    
    }

    public void AddCard(int cardPosInHand,Card card)
    {
        hand.Insert(cardPosInHand,card);

        playerHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;

        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(playerHandAnimation,hand));

        ChangeHandSize(handCardSize,hand);   
    }
    public void RemoveCard(int cardPosInHand)
    {
        hand.RemoveAt(cardPosInHand);
        playerHandAnimation.HandXAxisWidth = showingHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;
    }

    void SetCardsPosition(HandAnimationSettings animSett,List<Card> paramHand)
    {
        RectTransform rectCard;
        float WidthConst = animSett.HandXAxisWidth/ (paramHand.Count - 1);
        float concat = -animSett.HandXAxisWidth;
        int index = 0;

        foreach(Card card in paramHand)
        {          
            card.posInHand = index;
            rectCard = card.transform.parent as RectTransform;

            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition = new Vector2((paramHand.Count != 1 ? (concat * animSett.WidthMultiplier) + animSett.OffSet.x : 0) , animSett.OffSet.y + (-Mathf.Abs(concat)/(animSett.Angulation*15)));

            card.transform.parent.SetSiblingIndex(card.posInHand);

            //ROTATE ONLY THE IMAGE AND NOT THE GRAPHIC_COLLIDER
            rectCard = card.transform as RectTransform;

            card.startAngle = rectCard.transform.rotation;
            card.finalAngle = Quaternion.Euler(0,0,paramHand.Count > 2 ?(-concat/animSett.HandXAxisWidth) * animSett.Angulation : 0);

            concat += WidthConst * 2;   
            index ++;
        }
    }

    IEnumerator ShowInitializedCard(Card card)
    {
        //x its constant, y its smooth. both are 1 when the another be 1

        card.startAngle = Quaternion.Euler(0,90,90);
        card.finalAngle = Quaternion.identity;
       

        card.MoveTo(initCardAnimationSpeed,initCardXMaxCurvePos,initCardYMaxCurvePos);
        card.ChangeSize(showingHandSize,initCardAnimationSpeed);
            
        yield return new WaitForSeconds(initCardShowTime);

        hand.Add(card);
        
        playerHandAnimation.HandXAxisWidth = showingHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        
        if(transform.GetComponent<HandBoardInput>().pointerOnBoard)
        {
            ShowAmplifiedHand();
            SetCardsHandRaycast(true);
        }
        else
        {
            organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(playerHandAnimation,hand));
            ChangeHandSize(handCardSize,hand);
        }      
    }
    
    public IEnumerator OrganizeHand(HandAnimationSettings animSett,List<Card> paramHand)
    {
        RectTransform rectCard;
        float x,y;
        x = 0;
        
        SetCardsPosition(animSett,paramHand);

        while(x<=1)
        {
            x += (handAnimationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
           
            foreach(Card card in paramHand)
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

    void ChangeHandSize(float size,List<Card> paramHand)
    {
      
        for(int i = 0;i < paramHand.Count;i++)
        {       
            paramHand[i].ChangeSize(size,handAnimationSpeed);
        }      
             
    }
    public void ShowAmplifiedHand()
    {
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}     
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(showingHandAnimation,hand));

        ChangeHandSize(showingHandSize,hand);

    }
    public void StopShowingAmplifiedHand()
    {
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHand(playerHandAnimation,hand));

        ChangeHandSize(handCardSize,hand);
    }
    public void SetCardsHandRaycast(bool condition)
    {
        foreach(Card card in hand)
        {
            card.SetRaycastGraphic(condition);
        }
    }

    // organizar cartas 
    // remover carta da mão
    // adicionar carta da mão
    // adicionar carta ao campo
    // remover carta do campo
  
}
