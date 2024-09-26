using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
/// <summary>
/// 4.神父（精英怪）：体态臃肿的神父，脸部呈不详的紫红色。
/// 血量：60 
/// 近战伤害：5 
/// 行动力：1 
/// 行动模式：行走（每次走一格） 
/// 技能：在死亡时，在其3*3范围内召唤9只模糊血肉。如果3*3范围内存在障碍，则被障碍占据的格子不生成。
/// </summary>
public class Priest : EnemyBase
{
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 60;//最大生命值
        HP = 60;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 1;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "神父";//棋子名称
        chessDiscrption = "体态臃肿的神父，脸部呈不详的紫红色。\r\n在死亡时，在其3*3范围内召唤9只模糊血肉。如果3*3范围内存在障碍，则被障碍占据的格子不生成。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);

        BuffManager.instance.AddBuff("Priest_Buff", this);
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
