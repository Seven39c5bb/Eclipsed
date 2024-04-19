using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRelease : Card
{
    public override void CardFunc()
    {
        //
        BuffManager.instance.AddBuff("PowerReleaseBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
