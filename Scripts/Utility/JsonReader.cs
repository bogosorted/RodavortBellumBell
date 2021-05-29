using System;
using System.Collections.Generic;
using UnityEngine;


public class JsonReader:MonoBehaviour
{
    void Start()
    {
        //tests
        print(JsonReader.ReceiveLenguageTexts("eng").cards[1].description);
        print(JsonReader.ReceiveLenguageTexts("pt-br").cards[1].description);
    }
    [Serializable]
    public class TranslatedTexts
    {
        public Card[] cards;
        public MenuInfo[] menuInfo;
    }
    [Serializable]
    public class MenuInfo
    {
        public string initialButton;
        public string description;
    }
    [Serializable]
    public class Card
    {
        public string name;
        public string description;
    }

    public static TranslatedTexts ReceiveLenguageTexts(string Language)
    {
        TextAsset jsonInfo = Resources.Load<TextAsset>("Db/TextLanguage/" + Language);
        TranslatedTexts lenguageReader = JsonUtility.FromJson<TranslatedTexts>(jsonInfo.text);
        return lenguageReader;
    }
}
