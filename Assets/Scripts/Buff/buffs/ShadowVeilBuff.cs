using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowVeilBuff : BuffBase
{
    public override int BeCrashed(int damage, ChessBase attacker)
    {
        attacker.TakeDamage(damage / 2,attacker);
        return damage / 2;
    }
}
