using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Card
{
    public override void CardFunc()
    {
        //随机获取handCards中的一张卡牌，排除当前物体
        List<int> indexList = new List<int>();
        for (int i = 0; i < CardManager.instance.handCards.Count; i++)
        {
            if (CardManager.instance.handCards[i] != this.GetComponent<Card>())
            {
                indexList.Add(i);
            }
        }
        if(indexList.Count<2)
        {
            int index = Random.Range(0, indexList.Count);
            Card card = CardManager.instance.handCards[indexList[index]];
            indexList.RemoveAt(index);
            CardManager.instance.Discard(card);
            CardManager.instance.Draw(1);
            costManager.instance.curCost -= cost;
            return;
        }
        for (int i = 0; i < 2; i++)
        {
            int index=Random.Range(0, indexList.Count);
            Card card = CardManager.instance.handCards[indexList[index]];
            indexList.RemoveAt(index);
            CardManager.instance.Discard(card);
            CardManager.instance.Draw(1);
        }
        costManager.instance.curCost -= cost;
    }
}
