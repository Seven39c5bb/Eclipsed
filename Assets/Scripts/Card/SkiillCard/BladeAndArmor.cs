using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAndArmor : Card
{
    //获得基于你近战伤害数值的护盾
    public override void CardFunc()
    {
        PlayerController.instance.Barrier += PlayerController.instance.meleeAttackPower;
        costManager.instance.curCost -= cost;
    }
}
