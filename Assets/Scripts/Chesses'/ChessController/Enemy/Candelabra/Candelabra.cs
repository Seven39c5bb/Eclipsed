using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candelabra : EnemyBase
{
    public List<Vector2Int> points = new List<Vector2Int>();
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 100;//最大生命值
        HP = 100;//当前生命值
        meleeAttackPower = 0;//近战攻击力
        mobility = 1;//行动力
        moveMode = 3;//移动模式
        chessName = "烛台";//棋子名称
        //this.tag = "Enemy";
        chessDiscrption = "被攻击时照亮周围";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);

        BuffManager.instance.AddBuff("Candelabra_Buff", this);
    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }

        //用BFS算法移动
        //yield return base.OnTurn();

        yield return new WaitForSeconds(0.1f);

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
