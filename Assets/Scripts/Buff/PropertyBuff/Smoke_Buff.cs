using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Buff : BuffBase
{
    private void Awake()
    {
        buffNameCN = "烟雾中";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "无法被选中";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    public override bool OnPlayerUsePointerCardToEnemy()
    {
        Debug.Log("这个怪物不能被选中");
        return false;
    }
}
