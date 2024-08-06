using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docking : Card
{
    public override void CardFunc()
    {
        CardManager.instance.OpenDiscardUI(1);
        PlayerController.instance.barrier += 15;
        costManager.instance.curCost -= cost;
        ////随机获取handCards中的一张卡牌，排除当前物体
        //List<int> indexList = new List<int>();
        //for (int i = 0; i < CardManager.instance.handCards.Count; i++)
        //{
        //    if (CardManager.instance.handCards[i] != this.GetComponent<Card>())
        //    {
        //        indexList.Add(i);
        //    }
        //}
        //if (indexList.Count <=1)
        //{
        //    int index = Random.Range(0, indexList.Count);
        //    Card card = CardManager.instance.handCards[indexList[index]];
        //    indexList.RemoveAt(index);
        //    CardManager.instance.Discard(card);
        //    PlayerController.instance.Barrier += 15;
        //    costManager.instance.curCost -= cost;
        //    return;
        //}
        //else
        //{
        //    int index = Random.Range(0, indexList.Count);
        //    Card card = CardManager.instance.handCards[indexList[index]];
        //    indexList.RemoveAt(index);
        //    CardManager.instance.Discard(card);
        //    PlayerController.instance.Barrier += 15;
        //    costManager.instance.curCost -= cost;
        //}       
    }
}
