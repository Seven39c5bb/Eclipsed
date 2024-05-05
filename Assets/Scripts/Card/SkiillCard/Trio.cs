using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        float[] furthestDistances = new float[] { -1f, -1f, -1f };
        EnemyBase[] furthestEnemies = new EnemyBase[] { null, null, null };
        float nearestDistance = 1000f;
        int nearestIndex = 0;
        //对距离你最远的三个目标造成10点伤害。如果锁定到同一个目标，则造成10/15/20点伤害。
        for (int i = 0; i < 3; i++)
        {

            foreach (var enemy in ChessboardManager.instance.enemyControllerList)
            {
                // 如果 enemy 已经在 furthestEnemies 数组中，跳过这个 enemy
                if (furthestEnemies.Contains(enemy))
                {
                    continue;
                }
                //找到距离player最远的敌人
                float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                for (int j = 0; j < 3; j++)
                {
                    if (furthestDistances[j] == -1f)//必须先搜索完列表确定没有-1
                    {
                        furthestDistances[j] = dist;
                        furthestEnemies[j] = enemy;
                        if (dist < nearestDistance)
                        {
                            nearestDistance = dist;
                            nearestIndex = j;
                        }
                        break;
                    }
                }
                if (dist >= nearestDistance)
                {
                    furthestDistances[nearestIndex] = dist;
                    furthestEnemies[nearestIndex] = enemy;
                    nearestDistance = dist;
                    for (int j = 0; j < 3; j++)
                    {
                        if (furthestDistances[j] < nearestDistance)
                        {
                            nearestDistance = furthestDistances[j];
                            nearestIndex = j;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (furthestEnemies[i] == null)//如果该位置为null，说明敌人数量少于3个
            {
                if (furthestEnemies[i - 1] != null)
                {
                    StartCoroutine(Shot(furthestEnemies[i-1]));
                }
                else if (i - 2 >= 0 && furthestEnemies[i - 2] != null)
                {
                    StartCoroutine(Shot(furthestEnemies[i-2]));
                }
                else
                {
                    StartCoroutine(Shot(furthestEnemies[0]));
                }
                damage += 5;
                yield return new WaitForSeconds(0.51f);
                continue;
            }
            if (furthestEnemies[i].HP <= 0)
            {
                float furthestDistance = -1f;
                foreach (var enemy in furthestEnemies)
                {
                    //找到距离player最远的敌人,而且不为该furthestEnemy
                    float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                    if (dist > furthestDistance && furthestEnemies[i] != enemy)
                    {
                        furthestDistance = dist;
                        furthestEnemies[i] = enemy;
                    }
                }
            }
            if (furthestEnemies[i] != curEnemy)
            {
                if (furthestEnemies[i] != null)
                {
                    damage = 10;
                    StartCoroutine(Shot(furthestEnemies[i]));
                    curEnemy = furthestEnemies[i];
                }
            }
            else
            {
                if (furthestEnemies[i] != null)
                {
                    damage += 5;
                    StartCoroutine(Shot(furthestEnemies[i]));
                }
            }
            yield return new WaitForSeconds(0.51f);
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
