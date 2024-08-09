using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        //回合开始时，玩家护盾衰减
        PlayerController.instance.BarrierDecay();

        // 检查玩家buff
        List<BuffBase> buffsToProcess = new List<BuffBase>(PlayerController.instance.buffList);
        //检查玩家buff
        foreach (var buff in buffsToProcess)
        {
            buff.OnTurnStart();
        }

        FightManager.instance.turnCounter++;//回合数+1

        //遍历所有棋格，调用OnPlayerTurnBegin
        foreach(var cell in ChessboardManager.instance.cellStates)
        {
            cell.OnPlayerTurnBegin();
            //遍历所有棋格上的棋子
            if (cell.state == Cell.StateType.Occupied)
            {
                foreach(var buff in cell.occupant.GetComponent<ChessBase>()?.buffList)
                {
                    buff.OnPlayerTurnBegin();
                }
            }
        }


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

        // 回合开始时，抽牌结束后触发Buff
        FightManager.instance.StartCoroutine(DelayedOnTurnStartEndDrawFunc(0.71f));

        FightUI.instance.isEnemyTurn = false;
    }
    public override void OnUpdate()
    {
        //���Խ��в���
        //when deck count<=0,then update the deck
        if(CardManager.cardDesk.Count <= 0)
        {
            //Debug.Log("update the deck");
            CardManager.instance.UPdateDesk();
        }
        //����������������������
    }

    // 回合开始时，抽牌结束后触发Buff
    public IEnumerator DelayedOnTurnStartEndDrawFunc(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("OnTurnStartEndDrawFunc");
        List<BuffBase> buffsToProcess = new List<BuffBase>(PlayerController.instance.buffList);
        foreach (var buff in buffsToProcess)
        {
            buff.OnTurnStartEndDraw();
        }
    }
}
