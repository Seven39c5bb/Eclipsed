using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : Card
{
    //获得10护盾
    public override void CardFunc()
    {
        PlayerController.instance.Barrier += 8;
        costManager.instance.curCost -= cost;
    }
}
