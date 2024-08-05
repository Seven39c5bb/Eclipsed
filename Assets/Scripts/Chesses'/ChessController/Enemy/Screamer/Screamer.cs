using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /// <summary>
    /// 5.尖啸者（精英怪）：把头颅捧在怀里、右手抓着权杖的圣职者，可以发出尖锐的鸣叫声。 
    /// 血量：70 
    /// 近战伤害：5 
    /// 行动力：2 
    /// 行动模式：行走（每次走一格） 
    /// 被动技能：在玩家的回合开始时，玩家每打出一张牌，就会受到1点伤害。
    /// 主动技能：对自己5*5环形范围的玩家造成13点伤害。
    /// </summary>
public class Screamer : EnemyBase
{

    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 70;//最大生命值
        HP = 70;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "尖啸者";//棋子名称
        chessDiscrption = "把头颅捧在怀里、右手抓着权杖的圣职者，可以发出尖锐的鸣叫声。" +
            "\r\n被动技能：在玩家的回合开始时，玩家每打出一张牌，就会受到1点伤害。" +
            "\r\n主动技能：对自己5*5环形范围的玩家造成13点伤害。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);
        //在当前轮次，如果这个敌人受到过伤害，则行动力-1
        //BuffManager.instance.AddBuff("Screamer_Buff", PlayerController.instance);//添加buff

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
        ScreamerSkill();

        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

    private void ScreamerSkill()
    {
        //test
        Debug.Log("ScreamerSkill");
        BuffManager.instance.AddBuff("Screamer_Buff", PlayerController.instance);//添加buff
        //获取周围的敌人
        ChessBase player = null;
        //向ChessboardManager查询以自身为中心5*5范围内是否有玩家
        int leftAxis = location.x - 2 > 0 ? location.x - 2 : 0;
        int rightAxis = location.x + 2 < 9 ? location.x + 2 : 9;
        int upAxis = location.y - 2 > 0 ? location.y - 2 : 0;
        int downAxis = location.y + 2 < 9 ? location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessBase currCellObject = ChessboardManager.instance.CheckCell(new Vector2Int(i, j));
                if (currCellObject != null && currCellObject.tag == "Player")
                {
                    player.TakeDamage(13, this);
                }
            }
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
