using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshSacrificeBuff : BuffBase
{
    public override void OnTurnStart()
    {
        BuffManager.instance.AddBuff("SacrificeBuff", PlayerController.instance);
    }
}
