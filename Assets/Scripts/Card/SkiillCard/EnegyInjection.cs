using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnegyInjection : Card
{
    public override void CardFunc()
    {
        //添加一个注能buff
        BuffManager.instance.AddBuff("EnegyInjectionBuff", PlayerController.instance);
    }
}
