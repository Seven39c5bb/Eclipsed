using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorShapingBuff : BuffBase
{
    public override void OnAdd()
    {
        this.chessBase.MaxHp += 20;
        this.chessBase.Cure(20);
    }
    public override void OnTurnStart()
    {
        chessBase.meleeAttackPower += 2;
    }
}
