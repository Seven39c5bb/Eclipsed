using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshSacrifice : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("FleshSacrificeBuff", PlayerController.instance);
    }
}
