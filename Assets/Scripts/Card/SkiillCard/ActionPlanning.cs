using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionPlanning : Card
{
    //�������飬������cardTypeΪAction�Ŀ�Ƭ���뵽actionList��
    public List<Card> actionList = new List<Card>();
    public override void CardFunc()
    {
        foreach (var card in CardManager.cardDesk)
        {
            //
            string cardName = card;
            if (cardName.Contains("("))
            {
                int index = cardName.IndexOf("(");
                cardName = cardName.Substring(0, index);
            }
            if (Resources.Load("Prefabs/Card/" + cardName).GetComponent<Card>().type == cardType.action)
            {
                actionList.Add(Resources.Load("Prefabs/Card/" + cardName).GetComponent<Card>());
            }
        }
        //��actionList�еĿ�Ƭ���һ�ż��뵽������
        string drawCard= actionList[Random.Range(0, actionList.Count - 1)].name;
        FightUI.instance.InstantiateCard(1, drawCard);
        costManager.instance.curCost -= cost;
    }
}
