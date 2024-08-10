using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProperty : MonoBehaviour
{
    public Cell cell;
    public string propertyName;
    public string description;

    public virtual void OnChessEnter(ChessBase chess){ }
    public virtual void OnChessExit(ChessBase chess) { }
    public virtual void OnChessReach(ChessBase chess) { }
    public virtual void OnChessDepart(ChessBase chess) { }
    public virtual void OnPlayerTurnBegin() { }
    public virtual void OnAdd() { }
    public virtual void OnRemove() { }
}
