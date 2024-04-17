using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLoadingBuff : BuffBase
{
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
    public override void OnTurnEnd()
    {
        if(isActived)
        {
            BuffManager.instance.DeleteBuff(this.buffName, PlayerController.instance);
        }
        
    }
}
