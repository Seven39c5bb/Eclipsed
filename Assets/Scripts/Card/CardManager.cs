using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public static List<Card> cardDesk;
    public static List<Card> discardDesk;
    public List<Card> handCards;
    public int test = 1;

    //test 
    public GameConfig gameConfig;
    //test
    private void Awake()
    {
        instance = this;
        cardDesk = new List<Card>();
        discardDesk = new List<Card>();
        //��text�м���������Ϣ
        //......;0

        //test
        gameConfig= new GameConfig();
        gameConfig.Init();
        foreach(KeyValuePair<string,int> ele in gameConfig.cardDeckData)
        {
            string cardName=ele.Key;
            int cardCount=ele.Value;
            GameObject card = Resources.Load("Prefabs/Card/" + cardName) as GameObject;
            for(int i = 0; i < cardCount; i++)///test 5 �Ļ�(int)cardCount
            {
                cardDesk.Add(card.GetComponent<Card>());
            }      
        }
        //test
    }
    //抽卡
    public void Draw(int num)
    {
        for(int i=0;i<num;i++)
        {
            Card drawCard = cardDesk[Random.Range(0, cardDesk.Count - 1)];
            //��resource���ҵ��ÿ��ƣ����������ɵ���������
            FightUI.instance.InstantiateCard(1,drawCard.name);
            cardDesk.Remove(drawCard);
        }  
    }
    public void UPdateDesk()//���ÿ���
    {
        cardDesk = discardDesk;
        discardDesk.Clear();
    }
}
