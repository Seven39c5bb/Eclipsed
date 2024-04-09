using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConcentration_GunMan : BuffBase
{
    void Start()
    {
        buffName = "BuffConcentration_GunMan";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "射击伤害+3";
    }
}
