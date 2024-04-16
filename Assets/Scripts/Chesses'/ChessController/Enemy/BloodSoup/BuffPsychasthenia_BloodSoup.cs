using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPsychasthenia_BloodSoup : BuffBase
{
    void Awake()
    {
        buffName = "BuffPsychasthenia_BloodSoup";
        buffNameCN = "精神衰弱";
        description = "尖啸贯穿心扉，精神衰弱，卡牌费用减少";
        durationTurn = 9999;
        buffType = BuffType.Debuff;
        buffImgType = BuffImgType.Cost;
    }
    public override void OnAdd()
    {
        //对玩家卡牌费用进行修改
        costManager.instance.maxCost -= 1;
        if (costManager.instance.curCost > costManager.instance.maxCost)
        {
            costManager.instance.curCost = costManager.instance.maxCost;
        }
    }
    public override void OnRemove()
    {
        //对玩家卡牌费用进行修改
        costManager.instance.maxCost += 1;
    }
}
