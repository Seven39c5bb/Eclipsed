using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : EnemyBase
{//石像鬼
    /// <summary>
    /// 3.石像鬼：长得很像所谓石像鬼的飞行生物但头是骷髅，爪子相当锋利。
    /// 血量：20 
    /// 近战伤害：8 
    /// 行动力：2 
    /// 行动模式：飞行（每次走四格） 
    /// 技能：在当前轮次，如果这个敌人受到过伤害，则行动力-1
    /// </summary>
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 20;//最大生命值
        HP = 20;//当前生命值
        meleeAttackPower = 8;//近战攻击力
        mobility = 2;//行动力
        moveMode = 4;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "石像鬼";//棋子名称
        chessDiscrption = "长得很像所谓石像鬼的飞行生物但头是骷髅，爪子相当锋利。\r\n被动技能：在当前轮次，如果这个敌人受到过伤害，则行动力-1";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);
        //在当前轮次，如果这个敌人受到过伤害，则行动力-1
        BuffManager.instance.AddBuff("Gargoyle_Buff", this);//添加buff（行动力-1）

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

        //释放技能

        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

    public override bool IsInRange(Vector2Int playerLocation)//判断是否在该怪物偏好的环内
    {
        return false;//可直接用于碰撞类怪物,玩家位置既是偏好区
    }

    public override Vector2Int[] CellsInRange(Vector2Int playerLocation)//获取该怪物在玩家周围的偏好区，Location为玩家的位置
    {
        //返回Location
        return new Vector2Int[] { playerLocation };//可直接用于碰撞类怪物，玩家位置既是偏好区
    }
}
