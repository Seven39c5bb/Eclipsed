using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> cardDesk;
    public List<Card> discardDesk;
    public List<Card> handCards;
    public int test = 1;
    private void Awake()
    {
        instance = this;
        //cardDesk = new List<Card>();
        discardDesk = new List<Card>();
        Debug.Log(cardDesk.Count);
        //从text中加载牌组信息
        //......;
    }
    //抽卡
    public void Draw()
    {
        Card drawCard = cardDesk[Random.Range(0, cardDesk.Count - 1)];
        //从resource中找到该卡牌，并将其生成到手牌区中
        //InstantiateCard(drawCard.name);
        FightUI.instance.InstantiateCard(1);//test
        cardDesk.Remove(drawCard);
    }
    public void UPdateDesk()
    {
        cardDesk = discardDesk;
        discardDesk.Clear();
    }
}
