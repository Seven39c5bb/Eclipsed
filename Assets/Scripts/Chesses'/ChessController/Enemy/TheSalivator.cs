using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TheSalivator : EnemyBase
{//泌涎者
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 40;//最大生命值
        HP = 40;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 1;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "泌涎者";//棋子名称
        chessDiscrption = "头骨沿着眼眶向内塌陷的人类，从头颅的各个缝隙中流出五彩斑斓的液体。\r\n技能：分泌物射击：对玩家造成7点伤害，最大距离为3格。";//棋子描述
        ChessboardManager.instance.AddChess(this.gameObject, location);
        
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
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }

        //用BFS算法移动
        yield return base.OnTurn();
        

        //释放技能
        //Salivate();
        // 释放技能
        yield return StartCoroutine(ExecuteSkill(new SalivateSkill(this)));

        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

/*     public void Salivate()//唾液攻击
    {
        //获取周围的敌人
        ChessBase player = null;
        //向ChessboardManager查询以自身为中心7*7范围内是否有玩家
        int leftAxis = location.x - 3 > 0 ? location.x - 3 : 0;
        int rightAxis = location.x + 3 < 9 ? location.x + 3 : 9;
        int upAxis = location.y - 3 > 0 ? location.y - 3 : 0;
        int downAxis = location.y + 3 < 9 ? location.y + 3 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessBase currCellObject = ChessboardManager.instance.CheckCell(new Vector2Int(i, j));
                if (currCellObject != null && currCellObject.tag == "Player")
                {
                    player = currCellObject;
                    GameObject bulletEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletParticle");
                    GameObject bulletHitEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletHitEffect");
                    BulletAttack(7, player, bulletEffect, bulletHitEffect);
                    Debug.Log("泌涎者攻击了玩家，玩家受到了7点伤害");
                }
            }
        }
    } */

    public class SalivateSkill: SkillBase
    {
        public SalivateSkill(ChessBase chessBase) : base(chessBase)
        {
        }
        protected override IEnumerator ExecuteSkill()
        {
            // 执行子类特定的技能逻辑
            //获取周围的敌人
            ChessBase player = null;
            //向ChessboardManager查询以自身为中心7*7范围内是否有玩家
            int leftAxis = self.location.x - 3 > 0 ? self.location.x - 3 : 0;
            int rightAxis = self.location.x + 3 < 9 ? self.location.x + 3 : 9;
            int upAxis = self.location.y - 3 > 0 ? self.location.y - 3 : 0;
            int downAxis = self.location.y + 3 < 9 ? self.location.y + 3 : 9;
            for (int i = leftAxis; i <= rightAxis; i++)
            {
                for (int j = upAxis; j <= downAxis; j++)
                {
                    ChessBase currCellObject = ChessboardManager.instance.CheckCell(new Vector2Int(i, j));
                    if (currCellObject != null && currCellObject.tag == "Player")
                    {
                        player = currCellObject;
                        GameObject bulletEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletParticle");
                        GameObject bulletHitEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletHitEffect");
                        yield return self.BulletAttackAsync(7, player, bulletEffect, bulletHitEffect).AsCoroutine();
                        Debug.Log("泌涎者攻击了玩家，玩家受到了7点伤害");
                    }
                }
            }
            // 设置技能执行完成
            //yield return new WaitForSeconds(0.3f);
            IsCompleted = true;
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
        //返回9*9范围最外围的格子(记得设置条件不要返回已经被占据的格子和墙)
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        int leftAxis = Location.x - 3;
        int rightAxis = Location.x + 3;
        int upAxis = Location.y - 3;
        int downAxis = Location.y + 3;

        for (int i = leftAxis; i <= rightAxis; i++)
        {
            if(i == leftAxis || i == rightAxis) // 左列和右列
            {
                for (int j = upAxis; j <= downAxis; j++)
                {   
                    if(i >= 0 && i < 10 && j >= 0 && j < 10)
                    {
                        if((ChessboardManager.instance.cellStates[i, j].state != Cell.StateType.Occupied || ChessboardManager.instance.cellStates[i, j].occupant == this.gameObject)
                        && ChessboardManager.instance.cellStates[i, j].state != Cell.StateType.Wall)
                        {
                            result.Add(new Vector2Int(i, j));
                        }
                    }
                }
            }
            else // 上行和下行
            {
                if(upAxis >= 0 && upAxis < 10 && i >= 0 && i < 10)
                {
                    if((ChessboardManager.instance.cellStates[i, upAxis].state != Cell.StateType.Occupied || ChessboardManager.instance.cellStates[i, upAxis].occupant == this.gameObject)
                    && ChessboardManager.instance.cellStates[i, upAxis].state != Cell.StateType.Wall)
                    {
                        result.Add(new Vector2Int(i, upAxis));
                    }
                }
                if(downAxis >= 0 && downAxis < 10 && i >= 0 && i < 10)
                {
                    if((ChessboardManager.instance.cellStates[i, downAxis].state != Cell.StateType.Occupied || ChessboardManager.instance.cellStates[i, downAxis].occupant == this.gameObject)
                    && ChessboardManager.instance.cellStates[i, downAxis].state != Cell.StateType.Wall)
                    {
                        result.Add(new Vector2Int(i, downAxis));
                    }
                }
            }
        }
        
        return result.ToArray();//若复用，需要引入System.Linq，才能将HashSet转换为数组
    }
}
