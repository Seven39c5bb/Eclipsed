using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muddy_Buff : BuffBase
{
    EnemyBase enemyBase = null;
    int turnCount = 0;

    void Awake()
    {
        buffNameCN = "沾满泥巴";
        durationTurn = 1;
        buffType = BuffType.Debuff;
        description = "行动力减少1";
        canBeLayed = false;
        buffImgType = BuffImgType.Action;
    }

    public override void OnAdd()
    {
        // 如果是玩家棋子
        if (chessBase.gameObject.tag == "Player")
        {
            // 添加buff下次行动的格数减少1
            //chessBase.actionPoint -= 1;

            // 更改描述
            description = "下次行动的格数减少1";
        }
        else
        {
            // 如果是其他棋子
            // 添加buff，直到自己的下一个回合结束，行动力减少1
            enemyBase = chessBase as EnemyBase;
            enemyBase.mobility -= 1;

            // 更改描述
            description = "直到该单位的下一个回合结束，行动力减少1";
        }
    }
    public override void OnUnlayerBuffRepeatAdd()
    {
        turnCount = 0;
    }
    public override void OnTurnEnd()//仅对非玩家有效
    {   
        if (chessBase.gameObject.tag != "Player")
        {
            if (turnCount == durationTurn)
            {
                BuffManager.instance.DeleteBuff("Muddy_Buff", chessBase);
            }
            turnCount++;
        }
    }
    public override Vector2Int OnChessMove(Vector2Int direction)//仅对玩家有效
    {
        if (chessBase.gameObject.tag == "Player")
        {
            // 如果移动方向为0，直接返回
            if (direction == Vector2Int.zero)
            {
                return direction;
            }

            // 计算模长
            float magnitude = direction.magnitude;

            // 归一化向量
            Vector2 normalizedDirection = ((Vector2)direction).normalized;

            // 将模长减少1
            Vector2 newDirection = normalizedDirection * (magnitude - 1);

            // 将结果转换回 Vector2Int
            Vector2Int resultDirection = new Vector2Int(Mathf.RoundToInt(newDirection.x), Mathf.RoundToInt(newDirection.y));

            // 删除buff
            BuffManager.instance.DeleteBuff("Muddy_Buff", chessBase);

            return resultDirection;
        }
        return direction;
    }
    public override void OnRemove()
    {
        // 如果是玩家棋子
        if (chessBase.gameObject.tag == "Player")
        {
            // 移除buff下次行动的格数减少1
            //chessBase.actionPoint += 1;
        }
        else
        {
            // 如果是其他棋子
            // 移除buff，直到自己的下一个回合结束，行动力减少1
            enemyBase.mobility += 1;
        }
    }
}
