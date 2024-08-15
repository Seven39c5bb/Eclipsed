using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCell : Cell
{
    public override void OnChessEnter(ChessBase chess)
    {
        BuffManager.instance.AddBuff("Height_Buff", chess);
    }
}
