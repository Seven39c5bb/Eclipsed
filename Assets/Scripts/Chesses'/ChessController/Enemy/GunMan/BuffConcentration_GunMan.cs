using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConcentration_GunMan : BuffBase
{
    void Awake()
    {
        buffName = "BuffConcentration_GunMan";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "枪手全神贯注，射击伤害+3";
        canBeLayed = true;
    }
    public override void OnAdd()
    {
        GunMan gunMan = chessBase as GunMan;
        gunMan.shotDamage += 3;
    }
    public override void OnRemove()
    {
        GunMan gunMan = chessBase as GunMan;
        gunMan.shotDamage -= 3;
    }
}
