using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionFire : Card
{
    public int damage = 8;
    public override void CardFunc()
    {
        //找到血量最高的敌人
        EnemyBase maxHpEnemy = ChessboardManager.instance.enemyControllerList[0];
        //对3格外所有敌人造成8点伤害，再对血量最多的敌人造成12点伤害
        foreach (EnemyBase enemy in ChessboardManager.instance.enemyControllerList)
        {
            if(enemy.Hp>maxHpEnemy.Hp)
            {
                maxHpEnemy = enemy;
            }
            if(Mathf.Abs(PlayerController.instance.Location.x-enemy.Location.x)>=3||Mathf.Abs(PlayerController.instance.Location.y-enemy.Location.y)>=3)
            {
                enemy.TakeDamage(damage,PlayerController.instance);
            }
        }
        maxHpEnemy.TakeDamage(damage+4,PlayerController.instance);
    }
}
