using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region Attributes

    uint cardId;
    int life,gold,power;

    [Header("UI card references")]
    [SerializeField] Image design;
    [SerializeField] Text nameText,descText,lifeText,goldText,powerText;
     
    [HideInInspector] public Vector2 startSize;
    [HideInInspector] public Vector2 startPosition,finalPosition;
    [HideInInspector] public Quaternion startAngle,finalAngle;

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

    #endregion
}
