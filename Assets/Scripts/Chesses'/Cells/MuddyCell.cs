using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuddyCell : Cell
{
    public override void Awake()
    {
        // 填充墙体的名字和描述
        cellName = "泥泞地";
        cellDescription = "泥泞地上行走会减速";

        base.Awake();
    }

    public override void OnChessEnter(ChessBase chess)
    {
        // 如果是玩家棋子
        BuffManager.instance.AddBuff("Muddy_Buff", chess);
    }
}
