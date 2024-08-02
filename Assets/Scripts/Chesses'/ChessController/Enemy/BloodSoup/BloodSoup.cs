using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloodSoup : EnemyBase
{
    private static BloodSoup instance;
    public static BloodSoup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BloodSoup>();
            }
            return instance;
        }
    }
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = 180;//最大生命值
        HP = 180;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 1;//行动力
        moveMode = 2;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "血羹";//棋子名称
        chessDiscrption = "一大片血肉组成的“洼地”，其正中间漂浮着一具骷髅，仍保留着鲜红的眼球以及颅骨中的大脑，以及堆满胸腔的心脏。\r\n被动技能：血涌：血羹走过的地方会化为血水，玩家在血水上开始自己的回合时会受到9点伤害，血水持续两回合。\r\n被动技能：愈合：每回合恢复8点生命值。\r\n被动技能：精神衰竭：在生命值首次达到60以下后，使玩家本次战斗能使用的费用-1。\r\n被动技能：近朱者赤：血羹身边3*3范围的血池提升至15点，\r\n技能：血肉炮弹：向本回合开始时玩家所在的格子发射一枚炮弹，造成3*3范围的12点伤害，并留下血水。";//棋子描述
        dontMeleeAttack = true;//不主动碰撞

        //base.Start();//添加血条
        ChessboardManager.instance.AddChess(this.gameObject, location);

        //将以自己为中心3*3的格子设置为血池
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (location.x + i >= 0 && location.x + i < 10 && location.y + j >= 0 && location.y + j < 10)
                {
                    ChessboardManager.instance.cellStates[location.x + i, location.y + j].SetBloodPool(Cell.CellCondition.BloodPool_Deep);
                }
            }
        }
        BuffManager.instance.AddBuff("BuffCure_BloodSoup", this);
        StartCoroutine(CheckBossHealth());//检查boss生命值，第一次低于一半时触发灵魂尖啸
    }

    public override IEnumerator OnTurn()
    {
        // 检查所有棋格，如果cellCondition为BloodPool_Shallow，其持续回合-1，如果为0，将其设置为Normal
        foreach (var cell in ChessboardManager.instance.cellStates)
        {
            if (cell.cellCondition == Cell.CellCondition.BloodPool_Shallow)
            {
                cell.sustainTurn--;
                if (cell.sustainTurn == 0)
                {
                    cell.SetBloodPool(Cell.CellCondition.Normal);
                }
            }
        }
        //该敌人回合
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }
        BuffManager.instance.AddBuff("BuffCure_BloodSoup", this);

        //用BFS算法移动
        List<Vector2Int> path = ChessboardManager.instance.FindPath(location, PlayerController.instance.location, PlayerController.instance.gameObject, CellsInRange);
        if(path.Count == 1 || path == null || path.Count == 0)//如果已经到达偏好区，直接释放技能，防止下面访问path[1]时越界(path[0]是自己当前位置)
        {
            Debug.Log("无需移动");
        }
        else
        {
            Vector2Int nextDirection = path[1] - location;
            for (int i = 0; i < moveMode; i++)
            {
                //向玩家附近移动


                if(IsInRange(PlayerController.instance.location))//如果处在玩家周围偏好圈内，直接释放技能（防止怪物在偏好区内来回移动）
                {
                    break;
                }

                
                else
                {
                    Vector2Int preLocation = location;
                    Move(nextDirection);
                    //等待nextDirection的模*0.5f的时间后，再继续循环
                    float delay = 0.5f * (Mathf.Abs(nextDirection.x) + Mathf.Abs(nextDirection.y));
                    yield return new WaitForSeconds(delay);

                    //在此处更新棋格血池状态
                    //将以原先位置为中心3*3的格子设置为浅血池
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (preLocation.x + k >= 0 && preLocation.x + k < 10 && preLocation.y + j >= 0 && preLocation.y + j < 10)
                            {
                                ChessboardManager.instance.cellStates[preLocation.x + k, preLocation.y + j].SetBloodPool(Cell.CellCondition.BloodPool_Shallow);
                            }
                        }
                    }
                    //将以自己为中心3*3的格子设置为深血池
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (location.x + k >= 0 && location.x + k < 10 && location.y + j >= 0 && location.y + j < 10)
                            {
                                ChessboardManager.instance.cellStates[location.x + k, location.y + j].SetBloodPool(Cell.CellCondition.BloodPool_Deep);
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.3f);

        BuffManager.instance.AddBuff("BuffFleshLoad_BloodSoup", this);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

    public void OnBloodPool(int damage)//血池效果
    {
        PlayerController.instance.TakeDamage(damage, this);
    }

    //灵魂尖啸
    IEnumerator CheckBossHealth()
    {
        bool hasTriggered = false;
        while (true)
        {
            if (!hasTriggered && HP <= maxHp / 2)
            {
                hasTriggered = true;
                // 在这里触发你的效果
                BuffManager.instance.AddBuff("BuffPsychasthenia_BloodSoup", PlayerController.instance);
                Instantiate(Resources.Load("Prefabs/Particle/BloodSoup_BarkEffect"), this.transform.position, Quaternion.identity);
                yield break; // 停止当前协程
            }
            yield return new WaitForSeconds(0.5f); // 每隔0.5秒检查一次
        }
    }

    public override bool IsInRange(Vector2Int playerLocation)//判断是否在该怪物偏好的环内
    {
        Vector2Int[] aimRange = CellsInRange(playerLocation);


        foreach (Vector2Int cell in aimRange)
        {
            if (cell == playerLocation)
            {
                Debug.Log("IsInRange");
                return true;
            }
        }

        return false;
    }

    public override Vector2Int[] CellsInRange(Vector2Int playerLocation)//Location为玩家的位置
    {
        //返回9*9范围最外围的格子(记得设置条件不要返回已经被占据的格子和墙)
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        int leftAxis = playerLocation.x - 1;
        int rightAxis = playerLocation.x + 1;
        int upAxis = playerLocation.y - 1;
        int downAxis = playerLocation.y + 1;

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
