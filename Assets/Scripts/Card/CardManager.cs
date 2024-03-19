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
        //��text�м���������Ϣ
        //......;
    }
    //�鿨
    public void Draw()
    {
        Card drawCard = cardDesk[Random.Range(0, cardDesk.Count - 1)];
        //��resource���ҵ��ÿ��ƣ����������ɵ���������
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
