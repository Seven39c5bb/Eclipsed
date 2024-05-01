using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evilflame : Card
{
    public override void CardFunc()
    {
        PlayerController.instance.TakeDamage(5, PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
