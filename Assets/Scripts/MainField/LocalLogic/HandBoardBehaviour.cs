using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBoardBehaviour : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    List<Card> handOnBoard = new List<Card>();

    public void CreateCard(Card cardInfo)
    {
        GameObject newCard = Instantiate(cardPrefab,this.transform);
        Card cardAttributes = newCard.transform.GetChild(0).GetComponent<Card>();
        CardAnimation cardAnim = cardAttributes.transform.GetComponent<CardAnimation>();

        cardAttributes.ReceiveStartInfo(cardInfo.initialInfo);
        StartCoroutine(cardAnim.Dissolve(true));  
    }
}
