using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowVeilBuff : BuffBase
{
    void Awake()
    {
        buffName = "ShadowVeilBuff";
        buffNameCN = "暗影之幕";
        durationTurn = 1;
        buffType = BuffType.Buff;
        description = "你受到阴影庇护，所受到的伤害将有一半返还给攻击者";
        canBeLayed = true;
        buffImgType = BuffImgType.Defense;
    }
    public override int BeCrashed(int damage, ChessBase attacker)
    {
        attacker.TakeDamage(damage / 2,attacker);
        return damage / 2;
    }
}
