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
        //浠巘ext涓姞杞界墝缁勪俊鎭�
        //......;
    }
    //抽卡
    public void Draw(int num)
    {
        Card drawCard = cardDesk[Random.Range(0, cardDesk.Count - 1)];
        //浠巖esource涓壘鍒拌鍗＄墝锛屽苟灏嗗叾鐢熸垚鍒版墜鐗屽尯涓�
        //InstantiateCard(drawCard.name);
        FightUI.instance.InstantiateCard(num);//test
        cardDesk.Remove(drawCard);
    }
    public void UPdateDesk()
    {
        cardDesk = discardDesk;
        discardDesk.Clear();
    }
}
