using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BuffBase;

public class IncenseDemon_Draw_Buff : BuffBase
{
    private void Awake()
    {
        buffName = "IncenseDemon_Draw_Buff";
        buffNameCN = "潜行";
        durationTurn = 10;
        buffType = BuffType.Debuff;
        description = "使玩家下回合开始时抽牌数量-1";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    public override void OnBeforeDraw()
    {
        CardManager.instance.drawNum = 4;
    }
    public override void OnRemove()
    {
        CardManager.instance.drawNum = 5;
    }
}
