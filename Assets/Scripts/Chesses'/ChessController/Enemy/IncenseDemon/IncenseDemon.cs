using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int term = 1;
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
        chessDiscrption = "身上插满金属烛台的人形怪物，浓郁的焚香烟雾将其笼罩。";//棋子描述

        ChessboardManager.instance.AddChess(this.gameObject, location);

        Skill_1();
        StartCoroutine(CheckBossHealth());//检查boss生命值，第一次低于一半时触发灵魂尖啸
    }

    public override IEnumerator OnTurn()
    {
        // 该敌人回合
        List<BuffBase> tempBuffList = new List<BuffBase>(buffList); // 创建buffList的副本

        foreach (BuffBase buff in tempBuffList)
        {
            buff.OnTurnStart();
        }

        // 用BFS算法移动
        yield return base.OnTurn();

        yield return new WaitForSeconds(0.3f);
        switch (term)
        {
            case 1:
                Skill_2();
                break;
            case 2:
                Skill_3();
                break;
        }


        List<BuffBase> tempBuffList2 = new List<BuffBase>(buffList); // 创建buffList的副本
        foreach (BuffBase buff in tempBuffList2)
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

        //返回5*5范围内十字的格子(记得设置条件不要返回已经被占据的格子和墙)
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        //
        // 遍历直线位置
        for (int x = 0; x <= 9 - playerLocation.x ; x++)
        {
            if (x != playerLocation.x && Mathf.Abs(x-playerLocation.x)>2)
            {
                if ((ChessboardManager.instance.cellStates[x, playerLocation.y].state != Cell.StateType.Occupied
    || ChessboardManager.instance.cellStates[x, playerLocation.y].occupant == this.gameObject)
&& ChessboardManager.instance.cellStates[x, playerLocation.y].state != Cell.StateType.Wall)
                    result.Add(new Vector2Int(x, playerLocation.y));
            }
        }

        // 遍历横线位置
        for (int y = 0; y <= 9 - playerLocation.y; y++)
        {
            if (y != playerLocation.y && Mathf.Abs(y - playerLocation.y) > 2)
            {
                if ((ChessboardManager.instance.cellStates[y, playerLocation.x].state != Cell.StateType.Occupied
|| ChessboardManager.instance.cellStates[y, playerLocation.x].occupant == this.gameObject)
&& ChessboardManager.instance.cellStates[y, playerLocation.x].state != Cell.StateType.Wall)
                    result.Add(new Vector2Int(playerLocation.x, y));
            }
        }
        //返回Location
        return result.ToArray();//可直接用于碰撞类怪物，玩家位置既是偏好区
    }
    /// <summary>
    /// 被动1：用烟雾笼罩自身。只有进入BOSS的5*5范围内时，才能对其造成伤害。
    /// </summary>
    public void Skill_1()
    {
        //以自身5*5为中心生成烟雾
        //添加一个buff，当玩家进入boss为中心的5*5范围内，使boss为中心5*5范围内的烟雾消失，并且此时可以被选中
        BuffManager.instance.AddBuff("IncenseDemon_Buff", this);
    }
    /// <summary>
    /// 主动1：对自身十字范围内10*10的玩家造成10点伤害，并使玩家下回合开始时抽牌数量-1.
    /// </summary>
    public void Skill_2()
    {
        List<Vector2Int> checkPos = new List<Vector2Int>();
        for(int x = 0; x <= 9 - location.x; x++)
        {
            checkPos.Add(new Vector2Int(x,location.y));
        }
        for(int y = 0; y <= 9 - location.y; y++)
        {
            checkPos.Add(new Vector2Int(location.x,y));
        }
        foreach(var pos in checkPos)
        {
            if (PlayerController.instance.location == pos)
            {
                //特效
                //对玩家造成10点伤害
                PlayerController.instance.TakeDamage(10,this);
                //使玩家下回合开始时抽牌数量-1
                BuffManager.instance.AddBuff("IncenseDemon_Draw_Buff", PlayerController.instance);
            }
        }
        Debug.Log("触发Skill2");
    }

    ///<summary>
    /// 主动2：对5*5范围的玩家造成10点伤害。玩家下回合行动牌费用+10
    ///</summary>
    public void Skill_3()
    {
        Debug.Log("触发Skill3");
        int leftAxis = location.x - 2 > 0 ? location.x - 2 : 0;
        int rightAxis = location.x + 2 < 9 ? location.x + 2 : 9;
        int upAxis = location.y - 2 > 0 ? location.y - 2 : 0;
        int downAxis = location.y + 2 < 9 ? location.y + 2 : 9;
        //yield return new WaitForSeconds(1f);
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                if (ChessboardManager.instance.cellStates[i,j].state!=Cell.StateType.Empty &&
                    ChessboardManager.instance.cellStates[i, j].occupant.tag == "Player")
                {
                    PlayerController.instance.TakeDamage(10,this);
                    //使玩家下回合行动牌费用+10
                    break;
                }
            }
        }
    }
    IEnumerator CheckBossHealth()
    {
        bool hasTriggered = false;
        while (true)
        {
            if (!hasTriggered && HP <= maxHp / 2)
            {
                hasTriggered = true;
                // 在这里触发你的效果
                BuffManager.instance.DeleteBuff("IncenseDemon_Draw_Buff", PlayerController.instance);
                BuffManager.instance.DeleteBuff("IncenseDemon_Buff", this);
                term = 2;
                //特效

                yield break; // 停止当前协程
            }
            yield return new WaitForSeconds(0.5f); // 每隔0.5秒检查一次
        }
    }
}
