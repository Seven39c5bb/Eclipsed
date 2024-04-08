using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedBuff : BuffBase
{
    private void Start()
    {
        Debug.Log("增加攻击力");
        PlayerController.instance.meleeAttackPower += 10;
    }
    //当持续
}
