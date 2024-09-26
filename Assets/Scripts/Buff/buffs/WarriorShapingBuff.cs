using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorShapingBuff : BuffBase
{
    void Awake()
    {
        buffNameCN = "战士塑形";
        durationTurn = 1;
        buffType = BuffType.Buff;
        description = "你正在强健自己的体魄，每回合+2近战攻击力";
        canBeLayed = true;
        buffImgType = BuffImgType.Damage;
    }
    public override void OnAdd()
    {
        this.chessBase.maxHp += 12;
        this.chessBase.Cure(12);
    }
    public override void OnTurnStart()
    {
        chessBase.meleeAttackPower_private += 2;
    }
}
