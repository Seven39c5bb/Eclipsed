using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        Debug.Log("Player Trun now");
        //显示轮到玩家回合动画
        //抽五张牌
        CardManager.instance.Draw(1);//test
    }
    public override void OnUpdate()
    {
        //可以进行操作
        //如果牌库没有牌，将弃牌堆的牌全部放入卡组中
        if(CardManager.cardDesk.Count == 0)
        {
            CardManager.instance.UPdateDesk();
        }
        //将出牌区卡牌置入弃牌区
    }
}
