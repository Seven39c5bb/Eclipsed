using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorShapingBuff : BuffBase
{
    public override void OnAdd()
    {
        this.chessBase.MaxHp += 12;
        this.chessBase.Cure(12);
    }
    public override void OnTurnStart()
    {
        chessBase.meleeAttackPower += 1;
    }
}
