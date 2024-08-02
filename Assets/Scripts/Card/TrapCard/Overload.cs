using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Card
{
    public override void CardFunc()
    {
        //判断是否能使用该牌
        if (costManager.instance.curCost < cost)
        {
            this.isUsed = false;
            return;
        }
        CardManager.instance.OpenDiscardUI(2);
        CardManager.instance.Draw(2);
        costManager.instance.curCost -= cost;
    }
}