using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnegyInjection : Card
{
    public override void CardFunc()
    {
        //���һ��ע��buff
        BuffManager.instance.AddBuff("EnegyInjectionBuff", PlayerController.instance);
    }
}
