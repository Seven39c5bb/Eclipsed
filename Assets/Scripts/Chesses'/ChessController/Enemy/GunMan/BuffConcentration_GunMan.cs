using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConcentration_GunMan : BuffBase
{
    void Awake()
    {
        buffNameCN = "全神贯注";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "枪手全神贯注，射击伤害+";
        canBeLayed = true;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Damage;
    }
    public override void OnAdd()
    {
        GunMan gunMan = chessBase as GunMan;
        gunMan.shotDamage += 3;
        description = "枪手全神贯注，射击伤害+" + (gunMan.shotDamage - 5);
    }
    public override void OnRemove()
    {
        GunMan gunMan = chessBase as GunMan;
        gunMan.shotDamage -= 3;
        description = "枪手全神贯注，射击伤害+" + (gunMan.shotDamage - 5);
    }
}
