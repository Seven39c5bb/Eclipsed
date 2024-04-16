using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowVeil : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("ShadowVeilBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
