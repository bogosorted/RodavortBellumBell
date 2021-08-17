using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBehaviour : MonoBehaviour
{
    List<Card> hand = new List<Card>();

    const int MaxCardInHand = 10;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] public GameObject cardsToHand,cardsToBoard;
    
    [SerializeField] public bool isAdversaryPlayer;

    [Header("Initial Created Card Settings")]
    [SerializeField] float initCardAnimationSpeed;
    [SerializeField] float initCardShowTime,sizeInitShowCard,initCardYMaxCurvePos,initCardXMaxCurvePos,maxShowingAngle;
    [SerializeField] Vector2 startPosInitialCard,finalPosInitialCard;
    [SerializeField] float showingHandWidthMultiplier;

    
    [Header("Hand Card Settings")]
    
    [SerializeField] float handAnimationSpeed;  
    [SerializeField] float handXAxisWidth,maxHandAngle,handSizeIncreaseValue,ZangleOffSet;
    [SerializeField] Vector2 handOffset,showingHandOffset;
    [SerializeField] float handWidthMultiplier;

    [Header("Public Variables")]
    public float showingHandSize,handCardSize;
    
    HandAnimationSettings playerHandAnimation,showingHandAnimation;
    Coroutine organizeHandCurrentCoroutine;

    //the card only its added on final animation of initialized
    // that is the accurate hand.count
    [HideInInspector] public int handActualCount;

    public struct HandAnimationSettings
    {
        public HandAnimationSettings(Vector2 offSet,float zAngleOffSet,float angulation,float widthMultiplier,float handXWidth,bool playerHand)
        {
            OffSet = offSet;
            Angulation = angulation;
            ZAngleOffSet = zAngleOffSet;
            WidthMultiplier = widthMultiplier;
            HandXAxisWidth = handXWidth;
            PlayerHand = playerHand;
        }

        public Vector2 OffSet{get;}
        public float ZAngleOffSet{get;}
        public float Angulation{get;}
        public float WidthMultiplier{get;}
        public float HandXAxisWidth{get;set;}
        public bool PlayerHand{get;}
    }


    void Start()
    {
        
        playerHandAnimation = new HandAnimationSettings(handOffset,ZangleOffSet,maxHandAngle,handWidthMultiplier,handXAxisWidth,true);
        showingHandAnimation = new HandAnimationSettings(showingHandOffset,ZangleOffSet,maxShowingAngle,showingHandWidthMultiplier,handXAxisWidth,true);
        
        if(isAdversaryPlayer)
        {
        CreateCard();CreateCard();CreateCard();
        }
        else
        {
        CreateCard(1);CreateCard(1);CreateCard(1);
        }
        
            
        

    }
    void Update()
    {
        //test will be removed on realese
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(!isAdversaryPlayer)
                CreateCard(1);
            else
                CreateCard(0);
        }   
        
    }

    void CreateCard(uint index = 0) // if index == 0, the card will not have information (Enemy card)
    { 

        if(handActualCount < MaxCardInHand)
        {
            GameObject refCard = Instantiate(cardPrefab,cardsToHand.transform);
            Card newCard = refCard.transform.GetChild(0).GetComponent<Card>();
            handActualCount++;
            
            CardsInfo cardInfo = Resources.Load<CardsInfo>("Db/DbCardsAttributes/" + index.ToString());
            newCard.ReceiveStartInfo(cardInfo);

            if(index == 0)
            {
                newCard.GetComponent<Image>().sprite = cardInfo.design;
                newCard.transform.GetChild(0).gameObject.SetActive(false);
                newCard.transform.GetChild(1).gameObject.SetActive(false);
            }

            newCard.startPosition = startPosInitialCard;
            newCard.finalPosition = finalPosInitialCard;

            StartCoroutine(ShowInitializedCard(newCard));
        }    
    }

    public void AddCard(int cardPosInHand,Card card)
    {
        hand.Insert(cardPosInHand,card);

        playerHandAnimation.HandXAxisWidth = showingHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;

        OrganizeHand();

        ChangeHandSize(handCardSize,hand);   
    }
    public Card RemoveCard(int cardPosInHand)
    {
        Card removedCard = hand[cardPosInHand];

        hand.RemoveAt(cardPosInHand);
        playerHandAnimation.HandXAxisWidth = showingHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;

        return removedCard;
    }

    void SetCardsPosition(HandAnimationSettings animSett,List<Card> paramHand)
    {
        RectTransform rectCard;
        float xPos,yPos;
        float widthIncreaseConst = animSett.HandXAxisWidth/ (paramHand.Count - 1);
        float concat = -animSett.HandXAxisWidth;
        int index = 0;


        foreach(Card card in paramHand)
        {          
            card.posInHand = index;
            rectCard = card.transform.parent as RectTransform;

            xPos = paramHand.Count != 1 ? (concat * animSett.WidthMultiplier) + animSett.OffSet.x : 0;
            yPos = (-Mathf.Abs(concat)/(animSett.Angulation*15)) + animSett.OffSet.y;

            card.startPosition = rectCard.anchoredPosition;
            card.finalPosition = new Vector2(xPos , yPos);
                      
            card.transform.parent.SetSiblingIndex(card.posInHand);

            //ROTATE ONLY THE IMAGE AND NOT THE GRAPHIC_COLLIDER
            rectCard = card.transform as RectTransform;

            card.startAngle = rectCard.transform.rotation;
            card.finalAngle = Quaternion.Euler(0,0,paramHand.Count > 2 ?(-concat/animSett.HandXAxisWidth) * animSett.Angulation + animSett.ZAngleOffSet: animSett.ZAngleOffSet);

            concat += widthIncreaseConst * 2;   
            index ++;
        }
    }

    IEnumerator ShowInitializedCard(Card card)
    {
        //x its constant, y its smooth. both are 1 when the another be 1

        card.startAngle = Quaternion.Euler(0,90,90);
        card.finalAngle = Quaternion.Euler(0,0,playerHandAnimation.ZAngleOffSet);
       

        card.MoveTo(initCardAnimationSpeed,initCardXMaxCurvePos,initCardYMaxCurvePos);
        card.ChangeSize(showingHandSize,initCardAnimationSpeed);
            
        yield return new WaitForSeconds(initCardShowTime);

        card.transform.parent.SetParent(transform);
        hand.Add(card);

        
        playerHandAnimation.HandXAxisWidth = showingHandAnimation.HandXAxisWidth = (hand.Count-1) * handSizeIncreaseValue;
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        
        if(!isAdversaryPlayer && GetComponent<HandInput>().pointerOnBoard)
        {
            ShowAmplifiedHand();
            SetCardsHandRaycast(true);
        }
        else
        {
            organizeHandCurrentCoroutine = StartCoroutine(OrganizeHandAnim(playerHandAnimation,hand));
            ChangeHandSize(handCardSize,hand);
        }      
    }
    
    public void OrganizeHand()
    {
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHandAnim(playerHandAnimation,hand));
    }
    public IEnumerator OrganizeHandAnim(HandAnimationSettings animSett,List<Card> paramHand)
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
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHandAnim(showingHandAnimation,hand));

        ChangeHandSize(showingHandSize,hand);

    }
    public void StopShowingAmplifiedHand()
    {
        
        if (organizeHandCurrentCoroutine != null) {StopCoroutine(organizeHandCurrentCoroutine);}
        organizeHandCurrentCoroutine = StartCoroutine(OrganizeHandAnim(playerHandAnimation,hand));

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
