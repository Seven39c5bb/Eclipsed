using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        Debug.Log("Player Trun now");
        //��ʾ�ֵ���һغ϶���
        //��������
        for (int i = 0; i < 5; i++)
        {
            FightUI.instance.InstantiateCard(1);
        }
    }
    public override void OnUpdate()
    {
        //���Խ��в���
        //�غϽ������ճ��������Ƶ�˳�������Ϊ
        //����������������������
        //������������
    }
}
