using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Height_Buff : BuffBase
{
    void Awake()
    {
        buffNameCN = "居高临下";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "角色位于该地块时，使用远程技能牌命中敌人时每段多2点伤害。处于非高地的角色对位于高地的角色使用远程攻击时，有25%的概率miss。";
        canBeLayed = false;
        buffImgType = BuffImgType.Damage;
    }

    public override int OnHit(int damage, ChessBase target, DamageType damageType = DamageType.Null)
    {
        if (ChessboardManager.instance.cellStates[target.location.x, target.location.y] is HeightCell || damageType != DamageType.Remote)//如果目标在高地，或是远程攻击以外的攻击
        {
            return damage;
        }
        else
        {
            return damage + 2;
        }
    }

    public override int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        if (ChessboardManager.instance.cellStates[attacker.location.x, attacker.location.y] is HeightCell || damageType != DamageType.Remote)//如果攻击者在高地，或是远程攻击以外的攻击
        {
            return damage;
        }
        else//如果攻击者不在高度加成的格子上
        {
            //有25%概率闪避
            //掷骰子，如果数字小于6，则闪避
            int dice = FightManager.RollDice();
            if (dice < 6)
            {
                return 0;
            }
            else
            {
                return damage;
            }
        }
    }
}
