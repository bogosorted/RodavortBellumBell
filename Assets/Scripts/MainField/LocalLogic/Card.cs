using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region Attributes
    uint cardId;
    int life,gold;
   
    public Vector3 startPosition,finalPosition;
    public Quaternion startAngle,finalAngle;

    #endregion

    #region Properties


    Sprite Design
    {
        get => Design;
        set => transform.GetChild(0).GetComponent<Image>().sprite = value;
    }

    string Name
    {
        get => Name;
        set => transform.GetChild(1).GetComponent<Text>().text = value;
    }

    string Desc
    {
        get => Desc;
        set => transform.GetChild(2).GetComponent<Text>().text = value;
    }

    int Life
    {
        get => life;
        set {
            life = value;
            transform.GetChild(3).GetComponent<Text>().text = life.ToString();
        }
    }

    int Gold
    {
        get =>  gold;
        set {
            gold = value;
            transform.GetChild(4).GetComponent<Text>().text = gold.ToString();
        }
    }

    #endregion
    
    public void ReceiveStartInfo(CardsInfo info)
    {
        cardId = info.cardId;
        Life = info.life;
        Gold = info.gold;
        Design = info.design;
        Name = JsonReader.ReceiveLenguageTexts(Settings.lenguage).cards[cardId].name;
        Desc = JsonReader.ReceiveLenguageTexts(Settings.lenguage).cards[cardId].description;
    }
}
