using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trio : Card
{
    public int damage = 10;
    EnemyBase curEnemy= null;
    public override void CardFunc()
    {
        StartCoroutine(GenerateBullet());
        costManager.instance.curCost-= cost;
    }
    IEnumerator GenerateBullet()
    {
        //对距离你最远的三个目标造成10点伤害。如果锁定到同一个目标，则造成10/15/20点伤害。
        for (int i = 0; i < 3; i++)
        {

            //找到距离最远的目标
            float furthestDistance = -1f;
            EnemyBase furthestEnemy = null;
            foreach (var enemy in ChessboardManager.instance.enemyControllerList)
            {
                //找到距离player最远的敌人
                float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                if (dist > furthestDistance)
                {
                    furthestDistance = dist;
                    furthestEnemy = enemy;
                }
            }
            if (furthestEnemy.HP <= 0)
            {
                foreach (var enemy in ChessboardManager.instance.enemyControllerList)
                {
                    furthestDistance = -1f;
                    //找到距离player最远的敌人,而且不为该furthestEnemy
                    float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                    if (dist > furthestDistance && furthestEnemy != enemy)
                    {
                        furthestDistance = dist;
                        furthestEnemy = enemy;
                    }
                }
            }
            if (furthestEnemy != curEnemy)
            {
                damage = 10;
                StartCoroutine(Shot(furthestEnemy));
                curEnemy = furthestEnemy;
            }
            else
            {
                damage += 5;
                StartCoroutine(Shot(furthestEnemy));
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    IEnumerator Shot(EnemyBase enemy)
    {
        yield return new WaitForSeconds(0f);
        Debug.Log("yes");
        GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletParticle");
        GameObject HitEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletHitEffect");
        PlayerController.instance.BulletAttack(damage, enemy, BulletPrefab, HitEffect);
    }
}
