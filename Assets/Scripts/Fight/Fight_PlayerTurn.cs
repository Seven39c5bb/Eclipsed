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
        //CardManager.instance.Draw(5);
        FightUI.instance.InstantiateCard(7);
    }
    public override void OnUpdate()
    {
        //���Խ��в���
        //�غϽ������ճ��������Ƶ�˳�������Ϊ
        //����������������������
        //������������
    }
}
