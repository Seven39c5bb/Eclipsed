using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Buff : BuffBase
{
    public override bool OnPlayerUsePointerCardToEnemy()
    {
        Debug.Log("这个怪物不能被选中");
        return false;
    }
}
