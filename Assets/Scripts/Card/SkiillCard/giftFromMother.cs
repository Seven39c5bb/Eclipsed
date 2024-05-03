using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giftFromMother : Card
{
    public int damage;
    public float bulletInterval=0.1f; // 子弹间隔时间

    public override void CardFunc()
    {
        StartCoroutine(GenerateBullets());
        costManager.instance.curCost -= cost;
    }

    IEnumerator GenerateBullets()
    {
        for (int i = 0; i < 3; i++)
        {
            EnemyBase enemy = ChessboardManager.instance.enemyControllerList[Random.Range(0, ChessboardManager.instance.enemyControllerList.Count)];
            StartCoroutine(Shot(enemy));
            yield return new WaitForSeconds(bulletInterval); // 等待指定的时间间隔
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
