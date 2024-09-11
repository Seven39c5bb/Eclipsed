using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NUnit.Framework;

public class GunMan : EnemyBase
{
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 50;//最大生命值
        HP = 50;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 3;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "枪手";//棋子名称
        chessDiscrption = "头部变化为枪膛形状的行尸，能将骨刺从头部高速射出。\r\n技能：骨刺射击：对玩家造成5点伤害，最大距离为4格，下次使用该技能时伤害永久增加3点。";//棋子描述

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

        //释放技能
        //Shot();
        // 释放技能
        yield return StartCoroutine(ExecuteSkill(new ShotSkill(this)));


        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

    /* public int shotDamage = 5;//射击伤害
    public void Shot()//射击
    {
        //获取周围的敌人
        ChessBase player = null;
        //向ChessboardManager查询以自身为中心9*9范围内是否有玩家
        int leftAxis = location.x - 4 > 0 ? location.x - 4 : 0;
        int rightAxis = location.x + 4 < 9 ? location.x + 4 : 9;
        int upAxis = location.y - 4 > 0 ? location.y - 4 : 0;
        int downAxis = location.y + 4 < 9 ? location.y + 4 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessBase currCellObject = ChessboardManager.instance.CheckCell(new Vector2Int(i, j));
                if (currCellObject != null && currCellObject.gameObject.tag == "Player")
                {
                    player = currCellObject;
                    break;
                }
            }
        }

        if (player != null)
        {
            //特效加在这里
            GameObject bulletEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletParticle");
            GameObject bulletHitEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletHitEffect");
            BulletAttack(shotDamage, player, bulletEffect, bulletHitEffect);
            BuffManager.instance.AddBuff("BuffConcentration_GunMan", this);//添加全神贯注buff
        }
    } */

    public int shotDamage = 5;//射击伤害
    public class ShotSkill : SkillBase
    {
        public ShotSkill(ChessBase self) : base(self)
        {
        }
        protected override IEnumerator ExecuteSkill()
        {
            // 执行子类特定的技能逻辑
            //获取周围的敌人
            ChessBase player = null;
            //向ChessboardManager查询以自身为中心9*9范围内是否有玩家
            int leftAxis = self.location.x - 4 > 0 ? self.location.x - 4 : 0;
            int rightAxis = self.location.x + 4 < 9 ? self.location.x + 4 : 9;
            int upAxis = self.location.y - 4 > 0 ? self.location.y - 4 : 0;
            int downAxis = self.location.y + 4 < 9 ? self.location.y + 4 : 9;
            for (int i = leftAxis; i <= rightAxis; i++)
            {
                for (int j = upAxis; j <= downAxis; j++)
                {
                    ChessBase currCellObject = ChessboardManager.instance.CheckCell(new Vector2Int(i, j));
                    if (currCellObject != null && currCellObject.gameObject.tag == "Player")
                    {
                        player = currCellObject;
                        break;
                    }
                }
            }

            if (player != null)
            {
                //特效加在这里
                GameObject bulletEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletParticle");
                GameObject bulletHitEffect = Resources.Load<GameObject>("Prefabs/Particle/EnemyBulletParticle/EnemyBulletHitEffect");
                yield return self.BulletAttackAsync((self as GunMan).shotDamage, player, bulletEffect, bulletHitEffect).AsCoroutine();
                BuffManager.instance.AddBuff("BuffConcentration_GunMan", self);//添加全神贯注buff
            }
            yield return null;
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
        int leftAxis = Location.x - 4;
        int rightAxis = Location.x + 4;
        int upAxis = Location.y - 4;
        int downAxis = Location.y + 4;

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
