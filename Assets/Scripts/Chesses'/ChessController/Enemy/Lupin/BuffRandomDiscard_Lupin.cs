using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRandomDiscard_Lupin : BuffBase
{
    void Awake()
    {
        //初始化buff
        buffNameCN = "随机弃牌";
        durationTurn = 1;
        buffType = BuffType.Debuff;
        description = "被鲁宾的言语所蛊惑，你将在下个回合开始时随机弃掉手上3张牌。";
        canBeLayed = false;
        buffImgType = BuffImgType.Action;
    }
    public override void OnTurnStartEndDraw()
    {
        Card currCard;
        for (int i = 0; i < 3; i++)
        {
            currCard = CardManager.instance.handCards[Random.Range(0, CardManager.instance.handCards.Count)];
            CardManager.instance.Discard(currCard);
        }
        //删除buff
        BuffManager.instance.DeleteBuff(buffName, chessBase);
    }
}
