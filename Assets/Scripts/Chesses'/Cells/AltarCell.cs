using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarCell : Cell
{
    public override void Awake()
    {
        base.Awake();

        //设置地形名字和描述
        cellName = "祭坛";
        cellDescription = "时光凝铸的祭台，隐藏着禁忌的智慧。献祭如赌，以血换知，以命博力。";
    }

    public override void OnChessEnter(ChessBase chess)
    {
        BuffManager.instance.AddBuff("Altar_Buff", chess);
    }

    public override void OnChessExit(ChessBase chess)
    {
        Debug.Log("AltarCell OnChessExit");
        BuffManager.instance.DeleteBuff("Altar_Buff", chess);
    }
}
