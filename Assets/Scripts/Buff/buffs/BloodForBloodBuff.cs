using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodForBloodBuff : BuffBase
{
    public override int OnCrash(int damage, ChessBase target)
    {
        //当碰撞到目标时，恢复等同于该次伤害的生命值
        this.chessBase.Cure(damage);
        //销毁该buff
        BuffManager.instance.DeleteBuff(this.buffName, this.chessBase);
        return damage;
    }
}
