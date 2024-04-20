using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseEnhanced : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("SenseEnhancedBuff",PlayerController.instance);
        costManager.instance.curCost-= cost;
    }
}
