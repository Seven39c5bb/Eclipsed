using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedBuff : BuffBase
{
    public override void OnAdd()
    {
        this.chessBase=PlayerController.instance;
        Debug.Log("Ôö¼Ó¹¥»÷Á¦");
        PlayerController.instance.meleeAttackPower += 10;
    }
}
