using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedBuff : BuffBase
{
    public override void OnAdd()
    {
        this.chessBase=PlayerController.instance;
        Debug.Log("增加攻击力");
        PlayerController.instance.meleeAttackPower += 10;
    }
    public override void OnRemove()
    {
        Debug.Log("减少攻击力");
        PlayerController.instance.meleeAttackPower -= 10;
    }
}
