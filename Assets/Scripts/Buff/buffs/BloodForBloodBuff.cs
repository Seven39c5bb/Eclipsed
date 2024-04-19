using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodForBloodBuff : BuffBase
{
    void Awake()
    {
        buffName = "BloodForBloodBuff";
        buffNameCN = "以血还血";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "以血还血，当碰撞到目标时，恢复等同于该次伤害的生命值";
        canBeLayed = true;
        buffImgType = BuffImgType.HP;
    }
    public override int OnCrash(int damage, ChessBase target)
    {
        //当碰撞到目标时，恢复等同于该次伤害的生命值
        this.chessBase.Cure(damage);
        //销毁该buff
        BuffManager.instance.DeleteBuff(this.buffName, this.chessBase);
        return damage;
    }
}
