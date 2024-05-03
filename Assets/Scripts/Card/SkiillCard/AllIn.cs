using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AllIn : Card
{
    public override void CardFunc()
    {
        StartCoroutine(ReleaseFireballs());
    }

    private IEnumerator ReleaseFireballs()
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < CardManager.instance.handCards.Count; i++)
        {
            if (CardManager.instance.handCards[i] != this.GetComponent<Card>())
            {
                indexList.Add(i);
            }
        }

        costManager.instance.curCost -= cost;

        for (int i = 0; i < indexList.Count; i++)
        {
            Card card = CardManager.instance.handCards[indexList[i]];
            CardManager.instance.Discard(card);
            int damage = Random.Range(5, 11);
            float nearDistance = 9999f;
            EnemyBase nearEnemy = null;
            foreach (var enemy in ChessboardManager.instance.enemyControllerList)
            {
                float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                if (dist < nearDistance)
                {
                    nearDistance = dist;
                    nearEnemy = enemy;
                }
            }

            Vector3 fireballPosition = nearEnemy.transform.position + new Vector3(10, 0, -10);
            GameObject FireballPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerEffect/FireBall");
            GameObject fireball = Instantiate(FireballPrefab, fireballPosition, FireballPrefab.transform.rotation);

            fireball.transform.DOMove(nearEnemy.transform.position, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                nearEnemy.TakeDamage(damage, PlayerController.instance);
                Destroy(fireball);
            });

            yield return new WaitForSeconds(0.3f); // 在每次释放火球之间等待1秒
        }
    }
}
