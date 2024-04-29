using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllIn : Card
{
    public override void CardFunc()
    {
        //丢弃所有手牌。每丢弃一张牌，就对最近的敌人造成5-10点伤害。
        //随机获取handCards中的一张卡牌，排除当前物体
        List<int> indexList = new List<int>();
        for (int i = 0; i < CardManager.instance.handCards.Count; i++)
        {
            if (CardManager.instance.handCards[i] != this.GetComponent<Card>())
            {
                indexList.Add(i);
            }
        }
        for (int i = 0; i < indexList.Count; i++)
        {
            Card card = CardManager.instance.handCards[indexList[i]];
            CardManager.instance.Discard(card);
            int damage = Random.Range(5, 11);
            //找到距离最远的目标
            float nearDistance = 9999f;
            EnemyBase nearEnemy = null;
            foreach (var enemy in ChessboardManager.instance.enemyControllerList)
            {
                //找到距离player最远的敌人
                float dist = Mathf.Sqrt(Mathf.Pow(enemy.Location.x - PlayerController.instance.Location.x, 2) + Mathf.Pow(enemy.Location.y - PlayerController.instance.Location.y, 2));
                if (dist < nearDistance)
                {
                    nearDistance = dist;
                    nearEnemy = enemy;
                }
            }
            nearEnemy.TakeDamage(damage,PlayerController.instance);
        }
    }
}
