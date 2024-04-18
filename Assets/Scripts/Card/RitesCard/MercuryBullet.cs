using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("MercuryBulletBuff", PlayerController.instance);
    }
}
