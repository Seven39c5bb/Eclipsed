using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionPlanning : Card
{
    //遍历卡组，将所有cardType为Action的卡片加入到actionList中
    public List<Card> actionList = new List<Card>();
    int flag = 0;
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
                flag = 1;
            }
        }
        //如果卡组中没有action卡片，则直接返回
        if (flag == 0)
        {
            return;
        }
        //将actionList中的卡片随机一张加入到手牌中
        string drawCard= actionList[Random.Range(0, actionList.Count - 1)].name;
        FightUI.instance.InstantiateCard(1, drawCard);
        //从卡组中移除该卡片
        CardManager.cardDesk.Remove(drawCard);
        costManager.instance.curCost -= cost;
    }
}
