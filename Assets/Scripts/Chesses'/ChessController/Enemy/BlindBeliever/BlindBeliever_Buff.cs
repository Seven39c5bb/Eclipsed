using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BlindBeliever_Buff : BuffBase
{
    int damage = 10;
    void Awake()
    {
        buffName = "BlindBeliever_Buff";
        buffNameCN = "爆";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "在当前轮次，如果这个敌人受到过伤害，则行动力-1";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Damage;
    }
    public override void OnDie()
    {
        //释放特效
        //死亡时对3*3范围造成10点伤害。
        //检测该棋子3*3范围内有没有玩家
        //获取周围的敌人
        ChessBase player = null;
        //向ChessboardManager查询以自身为中心3*3范围内是否有玩家
        int leftAxis = chessBase.location.x - 1 > 0 ? chessBase.location.x - 1 : 0;
        int rightAxis = chessBase.location.x + 1 < 9 ? chessBase.location.x + 4 : 9;
        int upAxis = chessBase.location.y - 1 > 0 ? chessBase.location.y - 1 : 0;
        int downAxis = chessBase.location.y + 1 < 9 ? chessBase.location.y + 1 : 9;
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
        player.TakeDamage(10, chessBase);
        Debug.Log("爆了");

    }
}

