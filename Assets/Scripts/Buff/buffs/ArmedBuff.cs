using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedBuff : BuffBase
{
    private void Start()
    {
        Debug.Log("���ӹ�����");
        PlayerController.instance.meleeAttackPower += 10;
    }
    //������
}
