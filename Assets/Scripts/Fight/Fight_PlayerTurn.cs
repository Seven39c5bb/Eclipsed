using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        //检查玩家buff
        foreach(var buff in PlayerController.instance.buffList)
        {
            buff.OnTurnStart();
        }

        FightManager.instance.turnCounter++;//回合数+1
        Debug.Log("Player Trun now");
        UIManager.Instance.ShowTip("我的回合", Color.green, delegate ()
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
