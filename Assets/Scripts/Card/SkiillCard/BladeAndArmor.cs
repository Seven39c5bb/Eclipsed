using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAndArmor : Card
{
    //��û������ս�˺���ֵ�Ļ���
    public override void CardFunc()
    {
        PlayerController.instance.Barrier += PlayerController.instance.meleeAttackPower;
        costManager.instance.curCost -= cost;
    }
}
