using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashedPotato : EnemyBase
{//土豆泥
    void Awake()
    {
        // 初始化敌人棋子
        MaxHp = 50;//最大生命值
        HP = 50;//当前生命值
        MeleeAttackPower = 13;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "土豆泥";//棋子名称
        chessDiscrption = "五官被严重向内螺旋状扭曲的人类，大概已经死了。\r\n被动技能：强化肌肉：这个怪物的近战伤害增加3。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, Location);
        
    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }

        //用BFS算法移动
        yield return base.OnTurn();

        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }
    
}
