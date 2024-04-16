using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBoard : MonoBehaviour
{
    public enum boardType
    {
        copy,
        delete,
        showOnly
    }
    public List<CardTemplate> cardTemplates;
    public List<string> cardNames;
    public Transform cardTemplateParent;
    public boardType type;
    //从存档中读取卡牌信息
    private void Awake()
    {
        LoadDeck();
    }

    public void LoadDeck()
    {
        //从存档中读取卡牌信息
        cardNames= new List<string>(SaveManager.instance.jsonData.playerData.playerDeck);
        //将卡牌信息填充到cardTemplates中
        cardTemplates = new List<CardTemplate>();
        foreach (var cardName in cardNames)
        {
            //实例化一个卡牌模板
            GameObject cardObj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CardTemplate"), cardTemplateParent);
            CardTemplate cardTemplate = cardObj.GetComponent<CardTemplate>();
            cardTemplates.Add(cardTemplate);
            GameObject cardMes=Resources.Load<GameObject>("Prefabs/Card/"+cardName);
            //获取cardMes上信息
            if(cardMes!=null)
            {
                cardTemplate.card_Name = cardMes.name;
                //Debug.Log(cardMes.transform.GetChild(0).name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
            }
        }
    }
}
