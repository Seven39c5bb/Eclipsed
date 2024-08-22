using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBruisingBuff : BuffBase
{
    void Awake()
    {
        buffNameCN = "水银创伤";
        description = "该单位受到水银子弹的影响，受到的伤害+1";
        durationTurn = 9999;
        buffType = BuffType.Debuff;
        buffImgType = BuffImgType.HP;
    }
    public int brusingDmg = 1;
    public override int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        return damage + brusingDmg;
    }
}
