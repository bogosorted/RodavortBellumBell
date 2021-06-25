    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region Attributes

    uint cardId;
    int life,gold,power;
   
    Vector2 startSize;
    Coroutine changeSizeAnimation;
    
    [Header("UI card references")]
    [SerializeField] Image design;
    [SerializeField] Text nameText,descText,lifeText,goldText,powerText;

    [HideInInspector] public Vector2 startPosition,finalPosition;
    [HideInInspector] public Quaternion startAngle,finalAngle;
    [HideInInspector] public int posInHand;

    #endregion

    #region Properties

    int Life
    {
        get => life;
        set {
            life = value;
            lifeText.text = life.ToString();
        }
    }

    int Gold
    {
        get =>  gold;
        set {
            gold = value;
            goldText.text = gold.ToString();
        }
    }
     int Power
    {
        get =>  power;
        set {
            power = value;
            powerText.text = power.ToString();
        }
    }

    #endregion
    
    #region Methods

    public void ReceiveStartInfo(CardsInfo info)
    {
        cardId = info.cardId;
        Life = info.life;
        Gold = info.gold;
        Power = info.power;
        design.sprite = info.design;
        nameText.text = JsonReader.ReceiveLenguageTexts(UserPrefs.lenguage).cards[cardId].name;
        descText.text = JsonReader.ReceiveLenguageTexts(UserPrefs.lenguage).cards[cardId].description;
    }
    public void ChangeSize(float size,float animationSpeed)
    {
        startSize = transform.GetComponent<RectTransform>().localScale;
        if (changeSizeAnimation != null){StopCoroutine(changeSizeAnimation);}
            changeSizeAnimation = StartCoroutine(SizeAnimation(size,animationSpeed));
    }
    public IEnumerator SizeAnimation(float size,float animationSpeed)
    {
        RectTransform rectCard;
        float x,y;
        x = 0;
        while(x <= 1)
        { 
            x += (animationSpeed * Time.deltaTime);
            y = -x * x + 2 * x;
            rectCard = transform as RectTransform;
            rectCard.localScale = Vector3.one * Mathf.Lerp(startSize.x,size,y); 

            yield return null;
        }       
    }
    public void SetRaycastGraphic(bool condition)
    {
        transform.parent.GetComponent<Image>().raycastTarget = condition;
    }
    #endregion
}
