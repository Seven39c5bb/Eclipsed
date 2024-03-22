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
        //CardManager.instance.Draw(5);
        FightUI.instance.InstantiateCard(7);
    }
    public override void OnUpdate()
    {
        //可以进行操作
        //回合结束按照出牌区卡牌的顺序进行行为
        //将出牌区卡牌置入弃牌区
        //弃掉所有手牌
    }
}
