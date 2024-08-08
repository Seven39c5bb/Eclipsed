using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BlindBeliever : EnemyBase
{
    ///1.盲目信徒：两个眼眶全都被脓包填满的普通人。
    ///血量：35 
    ///近战伤害：5 
    ///行动力：3 
    ///行动模式：行走（每次走一格）
    ///技能：死亡时对3*3范围造成10点伤害。
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 30;//最大生命值
        HP = 30;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 3;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "盲目信徒";//棋子名称
        chessDiscrption = "两个眼眶全都被脓包填满的普通人。\r\n技能：死亡时对3*3范围造成10点伤害。";//棋子描述
        ChessboardManager.instance.AddChess(this.gameObject, location);
        //添加buff
        BuffManager.instance.AddBuff("BlindBeliever_Buff", this);
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
