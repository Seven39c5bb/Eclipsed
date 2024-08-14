using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lupin : EnemyBase
{//鲁邦
    private int currTurn = 0;
    public int lupinWallet;
    private static Lupin lupin_instance;
    public static Lupin instance
    {
        get
        {
            if (lupin_instance == null)
            {
                lupin_instance = GameObject.FindObjectOfType<Lupin>();
            }
            return lupin_instance;
        }
    }
    void Awake()
    {
        // 初始化敌人棋子
        maxHp = SaveManager.instance.jsonData.lupinData.MaxHP;//最大生命值
        HP = SaveManager.instance.jsonData.lupinData.HP;//当前生命值
        meleeAttackPower = 5;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "鲁邦";//棋子名称
        chessDiscrption = "在M市里四处偷盗、掠夺指骨的匪徒，身形高大、身披赤红色斗篷，似乎与商人是同一物种。\r\n技能1：对玩家造成3*4点伤害。自己每失去一百血，就再造成一次3点伤害。\r\n技能2：获得20点护盾。在下个回合开始时，对玩家造成护盾数额的伤害。\r\n技能3：对玩家造成10点伤害。在玩家回合开始时抽牌结束后，随机弃掉玩家手上3张牌。\r\n技能4：回合开始时，行动力变为5。\r\n技能5：结束战斗，并偷取玩家25-50点金钱。";//棋子描述


        ChessboardManager.instance.AddChess(this.gameObject, location);

    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }

        //行动前释放的技能
        switch (currTurn)
        {
            case 0:
                //技能1：对玩家造成3*4点伤害。自己每失去一百血，就再造成一次3点伤害。
                PlayerController.instance.TakeDamage(3 * ((maxHp - HP) / 100 + 4), this);
                break;
            case 1:
                //技能2：获得20点护盾。在下个回合开始时，对玩家造成护盾数额的伤害。
                barrier += 20;
                BuffManager.instance.AddBuff("BuffShieldCounter_Lupin", this);
                break;
            case 2:
                //技能3：对玩家造成10点伤害。在玩家回合开始时抽牌结束后，随机弃掉玩家手上3张牌。
                PlayerController.instance.TakeDamage(10, this);
                BuffManager.instance.AddBuff("BuffRandomDiscard_Lupin", PlayerController.instance);
                break;
            case 3:
                //技能4：回合开始时，行动力变为5。
                mobility = 5;
                break;
            case 4:
                //技能5：结束战斗，并偷取玩家25-50点金钱。
                int stealingTarget = Random.Range(25, 51);
                int originMoney = PlayerController.instance.coins;
                PlayerController.instance.coins -= stealingTarget;
                int stolenMoney = originMoney - PlayerController.instance.coins;
                lupinWallet += stolenMoney;
                //结束战斗，跳转到普通战斗结算页面
                FightManager.instance.ChangeType(FightType.Win);

                Debug.Log("鲁邦偷取了" + stolenMoney + "金币，现在共有" + lupinWallet + "金币。");
                yield break;
                //break;
            default:
                break;
        }

        //用BFS算法找出
        yield return base.OnTurn();


        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }

        currTurn++;
    }

    public override bool IsInRange(Vector2Int playerLocation)//判断是否在该怪物偏好的环内
    {
        return false;//可直接用于碰撞类怪物,玩家位置既是偏好区
    }

    public override Vector2Int[] CellsInRange(Vector2Int playerLocation)
    {

        // 用于存储路径
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(location);
        cameFrom[location] = location;

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        Vector2Int farthestCell = location;
        float maxDistance = 0;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            float currentDistance = Vector2Int.Distance(current, playerLocation);

            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                farthestCell = current;
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;
                if (!cameFrom.ContainsKey(next) && Vector2Int.Distance(location, next) <= mobility + 1)
                {
                    // 检查是否有障碍物
                    if (IsCellWalkable(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }
        }

        // 生成路径
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int step = farthestCell;
        while (step != location)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Reverse();

        // 检查路径是否为空
        if (path.Count == 0)
        {
            return null; // 或者 return new Vector2Int[0];
        }

        return new Vector2Int[] { path[0] };
    }

    // 检查格子是否可行走
    private bool IsCellWalkable(Vector2Int cell)
    {
        // 实现检查逻辑，例如检查是否有障碍物
        // 检查格子是否有障碍物
        if (cell.x < 0 || cell.x >= 10 || cell.y < 0 || cell.y >= 10)
        {
            return false;
        }
        else if (ChessboardManager.instance.cellStates[cell.x, cell.y].state == Cell.StateType.Empty)
        {
            return true;
        }
        return false;
    }
}


