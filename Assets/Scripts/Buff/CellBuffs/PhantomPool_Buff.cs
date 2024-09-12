using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomPool_Buff : BuffBase
{
    bool isPlayer = false;
    void Awake()
    {
        buffNameCN = "幻影池中";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "生命值损失的一半会被转化为护盾";
        canBeLayed = false;
        buffImgType = BuffImgType.Defense;
    }
    void Start()
    {
        //如果是玩家棋子
        if (chessBase.gameObject.tag == "Player")
        {
            isPlayer = true;
            description = "生命值损失的一半会被转化为护盾，但是你抽到的技能牌会被直接弃掉。";
        }
        else//如果是非玩家棋子
        {
            isPlayer = false;
            description = "生命值损失的一半会被转化为护盾，但是此单位无法使用技能";
        }
    }
    public override void OnAdd()
    {
        if (chessBase.gameObject.tag != "Player")
        {
            EnemyBase enemyBase = chessBase as EnemyBase;
            enemyBase.canExecuteSkill = false;
        }
    }
    public override void OnRemove()
    {
        if (chessBase.gameObject.tag != "Player")
        {
            EnemyBase enemyBase = chessBase as EnemyBase;
            enemyBase.canExecuteSkill = true;
        }
    }

    //在此地块时，生命值损失的一半会被转化为护盾，但是抽到的技能牌会被直接弃掉。（怪物进入此地形时，无法使用技能）
    public override void OnHPReduce(int damage)
    {
        if (chessBase.gameObject.tag == "Player")
        {
            //如果是玩家棋子
            //生命值损失的一半会被转化为护盾
            int barrier = damage / 2;
            chessBase.barrier += barrier;
            //更改描述
            description = "生命值损失的一半会被转化为护盾";
        }
        else
        {
            //如果是其他棋子
            //生命值损失的一半会被转化为护盾
            int barrier = damage / 2;
            chessBase.barrier += barrier;
            //更改描述
            description = "生命值损失的一半会被转化为护盾";
        }
    }

    public override void OnDrawCard(Card card)
    {
        if (card.type == Card.cardType.skill && isPlayer)
        {
            //如果是玩家棋子
            //抽到的技能牌会被直接弃掉
            CardManager.instance.Discard(card);
        }
    }
}
