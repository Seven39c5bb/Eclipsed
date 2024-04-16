using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReserveMagazine : Card
{
    //遍历卡组，将所有cardType为Skill的卡片加入到skillList中
    public List<Card> skillList = new List<Card>();
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
            if (Resources.Load("Prefabs/Card/" + cardName).GetComponent<Card>().type == cardType.skill)
            {
                skillList.Add(Resources.Load("Prefabs/Card/" + cardName).GetComponent<Card>());
            }
        }
        //将skillList中的最高费用卡片随机一张加入到手牌中
        //遍历skillList，找到费用最高的卡片
        int maxcost = -1,cindex=0;
        for(int i= 0;i < skillList.Count;i++)
        {
            if (skillList[i].cost > maxcost)
            {
                maxcost = skillList[i].cost;
                cindex = i;
            }
        }
        string drawCard = skillList[cindex].name;
        FightUI.instance.InstantiateCard(1, drawCard);
        //从卡组中移除该卡片
        CardManager.cardDesk.Remove(drawCard);
        costManager.instance.curCost -= cost;
    }
}
