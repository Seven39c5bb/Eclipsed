using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evilflameBuff : BuffBase
{
    public override void OnTurnEnd()
    {
        PlayerController.instance.TakeDamage(5, PlayerController.instance);
        BuffManager.instance.DeleteBuff("evilflameBuff", PlayerController.instance);
    }
}
