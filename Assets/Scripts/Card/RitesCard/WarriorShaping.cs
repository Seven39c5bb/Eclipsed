using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorShaping : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("WarriorShapingBuff", PlayerController.instance);
    }
    
}
