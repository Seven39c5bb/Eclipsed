using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShieldCounter_Lupin : BuffBase
{
    void Awake()
    {
        //初始化buff
        buffNameCN = "护盾反击";
        durationTurn = 1;
        buffType = BuffType.Buff;
        description = "鲁宾即将使用护盾反击，对玩家造成护盾数额的伤害。";
        canBeLayed = false;
        buffImgType = BuffImgType.Damage;
    }
    public override void OnTurnStart()
    {
        //对玩家造成护盾数额的伤害
        PlayerController.instance.TakeDamage(chessBase.barrier, chessBase);
        //删除buff
        BuffManager.instance.DeleteBuff(buffName, chessBase);
    }
}
