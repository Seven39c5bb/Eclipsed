using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProperty : MonoBehaviour
{
    public string propertyName;
    public string description;

    public virtual void OnChessEnter(ChessBase chess){ }
    public virtual void OnChessExit(ChessBase chess) { }
    public virtual void OnPlayerTurnBegin(ChessBase chess) { }
}
