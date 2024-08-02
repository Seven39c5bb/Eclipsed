using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashedPotato : EnemyBase
{//土豆泥
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 50;//最大生命值
        HP = 50;//当前生命值
        meleeAttackPower = 13;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "土豆泥";//棋子名称
        chessDiscrption = "五官被严重向内螺旋状扭曲的人类，大概已经死了。\r\n被动技能：强化肌肉：这个怪物的近战伤害增加3。";//棋子描述

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
    
}
