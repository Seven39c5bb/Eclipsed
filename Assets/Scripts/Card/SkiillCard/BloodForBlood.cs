using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodForBlood : Card
{
    public override void CardFunc()
    {
        //添加一个以血还血buff
        BuffManager.instance.AddBuff("BloodForBloodBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
