using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Clergy : EnemyBase
{
    /// <summary>
    /// 神职人员：被白色长袍遮盖身体的怪物，头的部分已经被巨型触手取代。
    /// 血量：80 
    /// 近战伤害：3 
    /// 行动力：2 
    /// 行动模式：行走（每次走一格）
    /// 技能：当这个怪物受到伤害时，为离他最近的怪物恢复5点生命值。
    /// </summary>
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 80;//最大生命值
        HP = 80;//当前生命值
        meleeAttackPower = 3;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "神职人员";//棋子名称
        chessDiscrption = "被白色长袍遮盖身体的怪物，头的部分已经被巨型触手取代。\r\n技能：当这个怪物受到伤害时，为离他最近的怪物恢复5点生命值。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);
        //在当前轮次，如果这个敌人受到过伤害，则行动力-1
        BuffManager.instance.AddBuff("Clergy_Buff", this);//添加buff（行动力-1）

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
