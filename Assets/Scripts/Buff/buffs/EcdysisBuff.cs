using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcdysisBuff : BuffBase
{
    public override void OnTurnStart()
    {
        PlayerController.instance.Cure(6);
    }
}
