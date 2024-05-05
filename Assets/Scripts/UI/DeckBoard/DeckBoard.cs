using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

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
                cardTemplate.cardCost.text= cardMes.GetComponent<Card>().cost.ToString();
                switch (cardMes.GetComponent<Card>().rare)
                {
                    case Card.rareType.common:
                        cardTemplate.cardName.color=Color.white;
                        break;
                    case Card.rareType.rare:
                        cardTemplate.cardName.color = Color.blue;
                        break;
                    case Card.rareType.legend:
                        cardTemplate.cardName.color = new Color(1, (float)(148 / 255), (float)(68 / 255));
                        break;
                }
                Sprite[] sprites = Resources.LoadAll<Sprite>("Pictures/CardImg/cardTypeImg");
                switch (cardMes.GetComponent<Card>().type)
                {
                    case Card.cardType.action:
                        cardTemplate.cardTypeImg.sprite = sprites[0];
                        break;
                    case Card.cardType.skill:
                        cardTemplate.cardTypeImg.sprite = sprites[4];
                        break;
                    case Card.cardType.rites:
                        cardTemplate.cardTypeImg.sprite = sprites[3];
                        break;
                    case Card.cardType.trap:
                        cardTemplate.cardTypeImg.sprite = sprites[5];
                        break;
                }
                //更换卡面
                if (Resources.Load("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName) != null)
                {
                    cardTemplate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName);
                }

            }
        }
    }
}
