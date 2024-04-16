using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLoading : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("QuickLoadingBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
