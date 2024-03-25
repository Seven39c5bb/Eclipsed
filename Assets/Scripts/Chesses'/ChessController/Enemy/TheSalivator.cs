using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TheSalivator : EnemyBase
{//泌涎者
    void Start()
    {
        // 初始化敌人棋子
        MaxHp = 40;//最大生命值
        HP = 40;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 1;//行动力
        moveModel = 1;//移动模式
        this.gameObject.tag = "Enemy";

        base.Start();//添加血条
        chessboardManager.AddChess(this.gameObject, Location);
        
    }

    /// <summary>
    /// 该敌人的回合
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// 外部调用方法：Coroutine currCoroutine = TheSalivator.StartCoroutine(TheSalivator.OnTurn());
    /// 可以通过currCoroutine == null判断是否在执行
    /// </remarks>
    public override IEnumerator OnTurn()
    {
        //该敌人回合

        //用BFS算法移动
        yield return base.OnTurn();
        

        //释放技能
        Salivate();

        yield return new WaitForSeconds(0.3f);
    }

    public void Salivate()//唾液攻击
    {
        //获取周围的敌人
        ChessBase player = null;
        //向ChessboardManager查询以自身为中心7*7范围内是否有玩家
        int leftAxis = Location.x - 3 > 0 ? Location.x - 3 : 0;
        int rightAxis = Location.x + 3 < 9 ? Location.x + 3 : 9;
        int upAxis = Location.y - 3 > 0 ? Location.y - 3 : 0;
        int downAxis = Location.y + 3 < 9 ? Location.y + 3 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessBase currCellObject = chessboardManager.CheckCell(new Vector2Int(i, j));
                if (currCellObject != null && currCellObject.tag == "Player")
                {
                    player = currCellObject;
                    player.TakeDamage(7);
                    Debug.Log("泌涎者攻击了玩家，玩家受到了7点伤害");
                }
            }
        }
    }


    public override bool IsInRange(Vector2Int Location)//判断是否在该怪物偏好的环内
    {
        Vector2Int[] aimRange = CellsInRange(Location);


        foreach (Vector2Int cell in aimRange)
        {
            if (cell == Location)
            {
                Debug.Log("IsInRange");
                return true;
            }
        }

        return false;
    }

    public override Vector2Int[] CellsInRange(Vector2Int Location)//Location为玩家的位置
    {
        //返回7*7范围最外围的格子(记得设置条件不要返回已经被占据的格子和墙)
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        int leftAxis = Location.x - 3;
        int rightAxis = Location.x + 3;
        int upAxis = Location.y - 3;
        int downAxis = Location.y + 3;
        for (int i = leftAxis; i <= leftAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {   
                if(i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if((chessboardManager.cellStates[i, j].state != Cell.StateType.Occupied || chessboardManager.cellStates[i, j].occupant == this.gameObject)
                    && chessboardManager.cellStates[i, j].state != Cell.StateType.Wall)//不返回已经被占据的格子和墙(会返回自己所在的格子)
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        for (int i = rightAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {   if(i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if((chessboardManager.cellStates[i, j].state != Cell.StateType.Occupied || chessboardManager.cellStates[i, j].occupant == this.gameObject)
                    && chessboardManager.cellStates[i, j].state != Cell.StateType.Wall)//不返回已经被占据的格子和墙(会返回自己所在的格子)
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= upAxis; j++)
            {
                if(i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if((chessboardManager.cellStates[i, j].state != Cell.StateType.Occupied || chessboardManager.cellStates[i, j].occupant == this.gameObject)
                    && chessboardManager.cellStates[i, j].state != Cell.StateType.Wall)//不返回已经被占据的格子和墙(会返回自己所在的格子)
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = downAxis; j <= downAxis; j++)
            {
                if(i >= 0 && i < 10 && j >= 0 && j < 10)
                {
                    if((chessboardManager.cellStates[i, j].state != Cell.StateType.Occupied || chessboardManager.cellStates[i, j].occupant == this.gameObject)
                    && chessboardManager.cellStates[i, j].state != Cell.StateType.Wall)//不返回已经被占据的格子和墙(会返回自己所在的格子)
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        
        return result.ToArray();//若复用，需要引入System.Linq，才能将HashSet转换为数组
    }
}
