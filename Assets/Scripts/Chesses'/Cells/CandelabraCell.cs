using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandelabraCell : Cell
{
    public override void Awake()
    {
        base.Awake();
        GameObject candelabra = Instantiate(Resources.Load("Prefabs/Chesses/Candelabra")) as GameObject;
        candelabra.GetComponent<ChessBase>().location = this.cellLocation;

        //烛台地块的名字和描述
        cellName = "烛台";
        cellDescription = "玩家攻击后可以驱散周围7*7范围的迷雾，持续3回合。";
    }
}
