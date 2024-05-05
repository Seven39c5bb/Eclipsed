using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecdysis : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("EcdysisBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
