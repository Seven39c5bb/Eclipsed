using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerReleaseBuff : BuffBase
{
    public override void OnDisCardCard(Card card)
    {
        foreach(var enemy in ChessboardManager.instance.enemyControllerList)
        {
            enemy.TakeDamage(4, PlayerController.instance);
        }
    }
}
