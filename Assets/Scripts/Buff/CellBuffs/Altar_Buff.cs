using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar_Buff : BuffBase
{
    void Awake()
    {
        //buffName = "Altar_Buff";
        buffNameCN = "祭坛之上";
        buffImgType = BuffImgType.Action;
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "在此地块回合开始时多抽一张牌，但是会扣除3点生命值。（怪物在此地形开始回合时，会多一点行动力，也扣3点血量）";
        canBeLayed = false;
    }

    void Start()
    {
        if (chessBase.gameObject.tag == "Player")
        {
            description = "在此地块回合开始时多抽一张牌，但是会扣除3点生命值";
        }
        else
        {
            description = "增加一点行动力，但是在回合开始时会扣除3点生命值";
        }
    }

    public override void OnTurnStart()//仅对玩家有效
    {
        if (chessBase.gameObject.tag == "Player")//如果是玩家
        {
            chessBase.TakeDamage(3, null, DamageType.Null);
            CardManager.instance.Draw(1);
        }
        else
        {
            chessBase.TakeDamage(3, null, DamageType.Null);
        }
    }

    //怪物在此地形开始回合时，会多一点行动力，也扣3点血量
    public override void OnAdd()
    {
        if (chessBase.gameObject.tag != "Player")
        {
            EnemyBase enemyBase = chessBase as EnemyBase;
            enemyBase.mobility += 1;
        }
    }
    public override void OnRemove()
    {
        if (chessBase.gameObject.tag != "Player")
        {
            EnemyBase enemyBase = chessBase as EnemyBase;
            enemyBase.mobility -= 1;
        }
    }
}
