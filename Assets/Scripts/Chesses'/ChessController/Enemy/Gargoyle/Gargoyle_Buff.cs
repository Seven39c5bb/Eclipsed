using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle_Buff : BuffBase
{
    void Awake()
    {
        buffName = "Gargoyle_Buff";
        buffNameCN = "飞驰疾行";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "在当前轮次，如果这个敌人受到过伤害，则行动力-1";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }

    public override int OnHurt(int damage, ChessBase attacker)
    {
        Gargoyle gargoyle = chessBase as Gargoyle;
        gargoyle.mobility -= 1;
        return damage;
    }
    public override void OnTurnEnd()
    {
        Gargoyle gargoyle = chessBase as Gargoyle;
        gargoyle.mobility = 2;//该怪回合结束后行动力恢复
    }
}
