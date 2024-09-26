using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLoadingBuff : BuffBase
{
    void Awake()
    {
        buffNameCN = "快速装填";
        durationTurn = 1;
        buffType = BuffType.Buff;
        description = "该回合中，玩家技能牌消耗减少1点";
        canBeLayed = false;
        buffImgType = BuffImgType.Cost;
    }
    public bool isActived = false;
    public override void OnTurnStart()
    {
        //该回合中，玩家技能牌消耗减少1点
        isActived = true;
    }
    public override void OnDrawCard(Card card)
    {
        if(isActived)
        {
            if(card.type == Card.cardType.skill)
            card.cost -= 1;
        }
    }
    public bool isRepeat = false;
    public override void OnTurnEnd()
    {
        if(isActived && !isRepeat)
        {
            BuffManager.instance.DeleteBuff(this.buffName, PlayerController.instance);
        }
        isRepeat = false;
    }
    public override void OnUnlayerBuffRepeatAdd()
    {
        isRepeat = true;//重复添加时，不会在回合末尾删除buff
    }
}
