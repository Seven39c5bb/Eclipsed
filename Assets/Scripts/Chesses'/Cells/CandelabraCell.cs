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
    }
}
