using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCell : Cell
{
    public override void Awake()
    {
        base.Awake();

        //设置地形名字和描述
        cellName = "高地";
        cellDescription = "站得高，看得远。";
    }
    public override void OnChessEnter(ChessBase chess)
    {
        BuffManager.instance.AddBuff("Height_Buff", chess);
    }
    public override void OnChessExit(ChessBase chess)
    {
        BuffManager.instance.DeleteBuff("Height_Buff", chess);
    }
}
