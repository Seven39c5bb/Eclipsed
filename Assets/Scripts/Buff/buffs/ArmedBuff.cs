using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedBuff : BuffBase
{
    private int increaseAttackPower = 0;
    void Awake()
    {
        buffNameCN = "武装";
        durationTurn = 9999;
        description = "近战攻击力提升了";
        canBeLayed = true;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Damage;
    }
    public override void OnAdd()
    {
        this.chessBase=PlayerController.instance;
        Debug.Log("增加攻击力");
        PlayerController.instance.meleeAttackPower_private += 5;
        increaseAttackPower += 5;
        description = "近战攻击力提升了" + increaseAttackPower + "点";
    }
    public override void OnRemove()
    {
        Debug.Log("减少攻击力");
        PlayerController.instance.meleeAttackPower_private -= 5;
        increaseAttackPower -= 5;
        description = "近战攻击力提升了" + increaseAttackPower + "点";
    }
}
