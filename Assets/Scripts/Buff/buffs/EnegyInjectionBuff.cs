using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnegyInjectionBuff : BuffBase
{
    public override int OnCrash(int damage, ChessBase target)
    {
        Debug.Log("����OnCarsh");
        /*���´���ײ��ɽ�ս�˺�ʱ��������е�����ɶ����ս�˺���*/
        foreach(var enemy in ChessboardManager.instance.enemyControllerList)
        {
            enemy.TakeDamage(damage, enemy);
        }
        //���ٸ�buff
        BuffManager.instance.DeleteBuff(this.name,PlayerController.instance);
        return damage;
    }

}
