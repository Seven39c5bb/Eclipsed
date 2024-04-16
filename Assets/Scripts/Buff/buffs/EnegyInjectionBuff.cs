using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnegyInjectionBuff : BuffBase
{
    void Awake()
    {
        buffName = "EnegyInjectionBuff";
        buffNameCN = "能量注入";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "你下次碰撞造成近战伤害时，会对所有敌人造成额外近战伤害。";
        canBeLayed = true;
        buffImgType = BuffImgType.Damage;
    }
    public override int OnCrash(int damage, ChessBase target)
    {
        Debug.Log("触发OnCarsh");
        /*你下次碰撞造成近战伤害时，会对所有敌人造成额外近战伤害。*/
        foreach(var enemy in ChessboardManager.instance.enemyControllerList)
        {
            enemy.TakeDamage(damage, enemy);
        }
        //销毁该buff
        BuffManager.instance.DeleteBuff(this.buffName,PlayerController.instance);
        return damage;
    }

}
