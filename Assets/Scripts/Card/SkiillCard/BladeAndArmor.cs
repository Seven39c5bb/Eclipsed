using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAndArmor : Card
{
    //��û������ս�˺���ֵ�Ļ���
    public override void CardFunc()
    {
        PlayerController.instance.barrier += PlayerController.instance.meleeAttackPower_private;
        costManager.instance.curCost -= cost;
    }
}
