using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeBuff : BuffBase
{
    public override void OnUseCard(Card card)
    {
        int hpCost = card.cost;
        card.cost = 0;
        PlayerController.instance.TakeDamage(hpCost,PlayerController.instance);
        BuffManager.instance.DeleteBuff("SacrificeBuff", PlayerController.instance);
    }
}
