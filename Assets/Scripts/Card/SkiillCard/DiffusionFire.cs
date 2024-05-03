using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionFire : Card
{
    public int damage = 8;
    public override void CardFunc()
    {
        GameObject HitEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletHitEffect");
        GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletParticle");
        //找到血量最高的敌人
        EnemyBase maxHpEnemy = ChessboardManager.instance.enemyControllerList[0];
        //对3格外所有敌人造成8点伤害，再对血量最多的敌人造成12点伤害
        foreach (EnemyBase enemy in ChessboardManager.instance.enemyControllerList)
        {
            if (enemy.Hp > maxHpEnemy.Hp)
            {
                maxHpEnemy = enemy;
            }
            if (Mathf.Abs(PlayerController.instance.Location.x - enemy.Location.x) >= 3 || Mathf.Abs(PlayerController.instance.Location.y - enemy.Location.y) >= 3)
            {
                //enemy.TakeDamage(damage,PlayerController.instance);
                PlayerController.instance.BulletAttack(damage, enemy, BulletPrefab, HitEffect);
            }
        }
        //maxHpEnemy.TakeDamage(damage+4,PlayerController.instance);
        StartCoroutine(DelayedBulletAttack(damage, maxHpEnemy, BulletPrefab, HitEffect));
    }

    private IEnumerator DelayedBulletAttack(int damage, EnemyBase enemy, GameObject bulletPrefab, GameObject hitEffect)
    {
        yield return new WaitForSeconds(0.2f); // 延迟1秒
        PlayerController.instance.BulletAttack(damage, enemy, bulletPrefab, hitEffect);
    }
}
