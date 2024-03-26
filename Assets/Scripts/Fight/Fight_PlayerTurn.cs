using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        Debug.Log("Player Trun now");
        UIManager.Instance.ShowTip("Player Turn", Color.green, delegate ()
        {
            Debug.Log("抽卡");
        });
        //��ʾ�ֵ���һغ϶���
        //update the cost
        costManager.instance.curCost = costManager.instance.maxCost;
        //draw card
        CardManager.instance.Draw(5);//test
        FightUI.instance.isEnemyTurn = false;
    }
    public override void OnUpdate()
    {
        //���Խ��в���
        //when deck count<=0,then update the deck
        if(CardManager.cardDesk.Count <= 0)
        {
            Debug.Log("update the deck");
            CardManager.instance.UPdateDesk();
        }
        //����������������������
    }
}
