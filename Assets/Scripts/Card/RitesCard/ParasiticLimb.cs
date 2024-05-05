using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiticLimb : Card
{
    public override void CardFunc()
    {
        PlayerController.instance.TakeDamage(8, PlayerController.instance);
        costManager.instance.maxCost += 1;
        costManager.instance.curCost -= cost;
    }
}
