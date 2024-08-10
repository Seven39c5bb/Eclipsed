using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 焚香恶鬼
/// 身上插满金属烛台的人形怪物，浓郁的焚香烟雾将其笼罩。
/// 血量：150 
/// 近战伤害：5 
/// 行动力：3 
/// 行动模式：行走（每次走一格） 
/// 技能：被动1：用烟雾笼罩自身。只有进入BOSS的5*5范围内时，才能对其造成伤害。
/// 主动1：对自身十字范围内10*10的玩家造成10点伤害，并使玩家下回合开始时抽牌数量-1.
/// 被动2：当自身血量低于一半时，BOSS身上的烛台全部剧烈燃烧，使被动1、主动1失效，
/// 并获得效果：怪物回合结束时，引燃一部分格子。当怪物回合开始时，引爆这些格子，如果玩家在被引爆的格子上，则会受到15点伤害。每回合被引燃的格子是交替的。
/// </summary>
public class IncenseDemon : EnemyBase
{
    public bool isInSmoke = false;
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 150;//最大生命值
        HP = 150;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 3;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "焚香恶鬼";//棋子名称
        chessDiscrption = "一团血肉混合物，仍在不停蠕动，似乎有着攻击与吞噬的本能。\r\n被动技能：强化肌肉：这个怪物的近战伤害增加3。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);

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

    public override bool IsInRange(Vector2Int playerLocation)//判断是否在该怪物偏好的环内
    {
        return false;//可直接用于碰撞类怪物,玩家位置既是偏好区
    }

    public override Vector2Int[] CellsInRange(Vector2Int playerLocation)//获取该怪物在玩家周围的偏好区，Location为玩家的位置
    {
        //返回Location
        return new Vector2Int[] { playerLocation };//可直接用于碰撞类怪物，玩家位置既是偏好区
    }
    /// <summary>
    /// 被动1：用烟雾笼罩自身。只有进入BOSS的5*5范围内时，才能对其造成伤害。
    /// </summary>
    public void Skill1()
    {
        //以自身5*5为中心生成烟雾
        int leftAxis = location.x - 2 > 0 ? location.x - 2 : 0;
        int rightAxis = location.x + 2 < 9 ? location.x + 2 : 9;
        int upAxis = location.y - 2 > 0 ? location.y - 2 : 0;
        int downAxis = location.y + 2 < 9 ? location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessboardManager.instance.ChangeProperty(new Vector2Int(i, j), "Smoke");
            }
        }
        //添加一个buff，当玩家进入boss为中心的5*5范围内，使boss为中心5*5范围内的烟雾消失，并且此时可以被选中
    }
}
