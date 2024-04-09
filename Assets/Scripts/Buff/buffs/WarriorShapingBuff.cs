using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorShapingBuff : BuffBase
{
    public override void OnAdd()
    {
        this.chessBase.HP += 20;
    }
    public override void OnTurnStart()
    {
        chessBase.meleeAttackPower += 10;
    }
}
