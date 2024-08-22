using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Screamer_Buff : BuffBase
{
    void Awake()
    {
        buffNameCN = "恐惧";
        durationTurn = 10;
        buffType = BuffType.Debuff;
        description = "玩家每打出一张牌，就会受到1点伤害";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    //玩家每打出一张牌，就会受到1点伤害
    public override void OnUseCard(Card card)
    {
        PlayerController.instance.TakeDamage(1,null);
    }
}
