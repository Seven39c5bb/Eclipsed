using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wisdom : Card
{
    public override void CardFunc()
    {
        CardManager.instance.Draw(2);
        costManager.instance.curCost -= cost;
    }
}
