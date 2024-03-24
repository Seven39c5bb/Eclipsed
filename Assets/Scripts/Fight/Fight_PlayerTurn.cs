using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        Debug.Log("Player Trun now");
        //��ʾ�ֵ���һغ϶���
        //�ָ�����
        costManager.instance.curCost = costManager.instance.maxCost;
        //��������
        CardManager.instance.Draw(1);//test
    }
    public override void OnUpdate()
    {
        //���Խ��в���
        //����ƿ�û���ƣ������ƶѵ���ȫ�����뿨����
        if(CardManager.cardDesk.Count == 0)
        {
            CardManager.instance.UPdateDesk();
        }
        //����������������������
    }
}
