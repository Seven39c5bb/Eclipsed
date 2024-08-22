using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Clergy_Buff : BuffBase
{
    void Awake()
    {
        buffNameCN = "神圣治疗";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "当这个怪物受到伤害时，为离他最近的怪物恢复5点生命值。";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Defense;
    }

    public override int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        //找到离该棋子最近的怪物
        float minDistance = 1000f;
        EnemyBase enemyBase = null;
        foreach(EnemyBase enemy in ChessboardManager.instance.enemyControllerList)
        {
            if (enemy == chessBase)
                continue;
            if (Vector2Int.Distance(enemy.location, chessBase.location) < minDistance)
            {
                minDistance = Vector2Int.Distance(enemy.location, chessBase.location);
                enemyBase = enemy;
            }
        }
        enemyBase.Cure(5);
        Debug.Log("为" + enemyBase.name + "恢复5点生命值");
        return damage;
    }

}
