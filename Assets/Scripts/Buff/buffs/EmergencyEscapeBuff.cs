using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyEscapeBuff : BuffBase
{
    public override void OnUseCard(Card card)
    {
        if (card.type == Card.cardType.action)
        {
            card.isUsed = false;
        }
    }
    public override void OnTurnEnd()
    {
        BuffManager.instance.DeleteBuff("EmergencyEscapeBuff", PlayerController.instance);
    }
}
