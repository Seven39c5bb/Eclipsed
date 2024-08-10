using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCell : Cell
{
    public override void OnChessEnter(ChessBase chess)
    {
        if(state == StateType.Occupied && occupant.tag == "Player")
        {
            PlayerController.instance.TakeDamage(10, null);
        }
    }
}
