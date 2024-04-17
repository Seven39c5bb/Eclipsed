using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercuryBulletBuff : BuffBase
{
    //你每使用一张技能牌后，使怪物受到的所有伤害+1。
    public override void OnUseCard(Card card)
    {
        if (card.type == Card.cardType.skill)
        {
            foreach(var enemy in ChessboardManager.instance.enemyControllerList)
            {
                //检测敌人上的buffList有没有EnemyBruisingBuff
                bool hasBuff = false;
                foreach(var buff in enemy.buffList)
                {
                    if(buff.buffName == "EnemyBruisingBuff")
                    {
                        hasBuff = true;
                        break;
                    }
                }
                if(hasBuff == false)
                {
                    BuffManager.instance.AddBuff("EnemyBruisingBuff", enemy);
                }
                else
                {
                    foreach (var buff in enemy.buffList)
                    {
                        if (buff.buffName == "EnemyBruisingBuff")
                        {
                            buff.GetComponent<EnemyBruisingBuff>().brusingDmg++;
                        }
                    }
                }
            }
        }
    }
}
