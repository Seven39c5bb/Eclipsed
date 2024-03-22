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
        //鍙互杩涜鎿嶄綔
        //鍥炲悎缁撴潫鎸夌収鍑虹墝鍖哄崱鐗岀殑椤哄簭杩涜琛屼负
        //灏嗗嚭鐗屽尯鍗＄墝缃叆寮冪墝鍖�
        //寮冩帀鎵�鏈夋墜鐗�
    }
}
