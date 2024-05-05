using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyEscapeBuff : BuffBase
{
    public override void OnAdd()
    {
        foreach(Card card in CardManager.instance.handCards)
        {
            if(card.type == Card.cardType.action)
            {
                card.cost += 10;
            }
        }
    }
    //public override void OnUseCard(Card card)
    //{
    //    if (card.type == Card.cardType.action)
    //    {
    //        card.cost += 10;
    //    }
    //}
    public override void OnTurnEnd()
    {
        BuffManager.instance.DeleteBuff("EmergencyEscapeBuff", PlayerController.instance);
    }
}
