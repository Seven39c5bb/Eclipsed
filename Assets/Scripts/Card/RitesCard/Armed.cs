using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armed : Card
{
    public override void CardFunc()
    {
        Instantiate(Resources.Load("Prefabs/Buff/ArmedBuff"));
    }
}
