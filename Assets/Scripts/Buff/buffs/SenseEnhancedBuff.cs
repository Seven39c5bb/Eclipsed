using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseEnhancedBuff : BuffBase
{
    public override void OnTurnStart()
    {
        CardManager.instance.Draw(2);
    }
}
