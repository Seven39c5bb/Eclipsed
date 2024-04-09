using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Armed : Card
{
    public override void CardFunc()
    {
        BuffManager.instance.AddBuff("ArmedBuff", PlayerController.instance);
    }
}
