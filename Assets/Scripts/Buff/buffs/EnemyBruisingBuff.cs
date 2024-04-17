using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBruisingBuff : BuffBase
{
    public int brusingDmg = 1;
    public override int OnHurt(int damage, ChessBase attacker)
    {
        return damage + brusingDmg;
    }
}
