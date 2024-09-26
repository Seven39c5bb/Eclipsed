using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomPoolCell : Cell
{
    public override void Awake()
    {
        base.Awake();

        //设置地形名字和描述
        cellName = "幻影池";
        cellDescription = "水波荡漾，虚实难辨。这里的倒影不仅映照外貌，更能窥见内心。伤痛化作壁障，却也模糊了本真。在这迷离的水域中，技艺失效，唯有本能方能引领前路。";
    }
    public override void OnChessEnter(ChessBase chess)
    {
        BuffManager.instance.AddBuff("PhantomPool_Buff", chess);
    }
    public override void OnChessExit(ChessBase chess)
    {
        BuffManager.instance.DeleteBuff("PhantomPool_Buff", chess);
    }
}
