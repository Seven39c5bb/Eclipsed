using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorGangster : EnemyBase
{
    void Awake()
    {
        // 初始化敌人棋子
        MaxHp = 70;//最大生命值
        HP = 70;//当前生命值
        MeleeAttackPower = 8;//近战攻击力
        mobility = 2;//行动力
        moveMode = 2;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "摩托暴徒";//棋子名称
        chessDiscrption = "身体发生诡异膨胀的摩托骑手，手上抓着一具干尸作为武器。\r\n被动技能：飞驰疾行：如果在上个回合没有受到伤害，则行动力+1。\r\n技能：暴力锤击：对3*3范围内的玩家造成15点伤害。";//棋子描述
        ChessboardManager.instance.AddChess(this.gameObject, Location);
        BuffManager.instance.AddBuff("BuffDriveMotor_MotorGangster", this);
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

        yield return new WaitForSeconds(0.2f);
        //释放技能
        RangeInjury();
        yield return new WaitForSeconds(0.3f);
        BuffManager.instance.AddBuff("BuffDriveMotor_MotorGangster", this);//刷新驾驶buff

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }

    public void RangeInjury()//范围伤害
    {
        //获取周围的敌人
        ChessBase player = null;
        //实例化旋风斩特效
        GameObject effect = Instantiate(Resources.Load<GameObject>("Prefabs/Particle/Whirlwind Slash Effect"), transform.position, Quaternion.identity);
        //向ChessboardManager查询以自身为中心3*3范围内是否有玩家
        int leftAxis = Location.x - 1 > 0 ? Location.x - 1 : 0;
        int rightAxis = Location.x + 1 < 9 ? Location.x + 1 : 9;
        int upAxis = Location.y - 1 > 0 ? Location.y - 1 : 0;
        int downAxis = Location.y + 1 < 9 ? Location.y + 1 : 9;
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
            //对玩家造成伤害
            //特效加在这里
            player.TakeDamage(15, this);
        }
    }
}
