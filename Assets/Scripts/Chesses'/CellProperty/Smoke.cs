using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : CellProperty
{
    private void Awake()
    {
        this.propertyName = "Smoke";
        this.description = "遮挡物块";
    }

    public override void OnChessEnter(ChessBase chess)
    {
        Debug.Log("Enter in the Smoke");
        Color oriColor = chess.GetComponent<SpriteRenderer>().color;
        chess.GetComponent<SpriteRenderer>().DOColor(new Color(oriColor.r, oriColor.g, oriColor.b, 0), 0.3f);
    }
    public override void OnChessExit(ChessBase chess)
    {
        Debug.Log("Exit the Smoke");
        chess.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f);
    }
}
