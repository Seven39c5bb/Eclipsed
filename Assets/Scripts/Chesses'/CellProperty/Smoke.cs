using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        chess.GetComponent<SpriteRenderer>().DOColor(new Color(oriColor.r, oriColor.g, oriColor.b, 0.5f), 0.3f);
        BuffManager.instance.AddBuff("Smoke_Buff", cell.occupant.GetComponent<ChessBase>());
    }
    public override void OnChessExit(ChessBase chess)
    {
        Debug.Log("Exit the Smoke");
        chess.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f);
        BuffManager.instance.DeleteBuff("Smoke_Buff", chess);
    }
    public override void OnAdd()
    {
        //判断添加时该棋格上有没有棋子
        if(cell.state==Cell.StateType.Occupied)
        {
            cell.occupant.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0.5f), 0.3f);
            BuffManager.instance.AddBuff("Smoke_Buff", cell.occupant.GetComponent<ChessBase>());
        }
    }
    public override void OnRemove()
    {
        //判断移除时该棋格上有没有棋子
        if (cell.state == Cell.StateType.Occupied)
        {
            cell.occupant.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f).OnComplete(() =>
            {
                cell.property = null;
                BuffManager.instance.DeleteBuff("Smoke_Buff", cell.occupant.GetComponent<ChessBase>());
                Destroy(this.gameObject);
            });
        }
        else
        {
            cell.property = null;
            //BuffManager.instance.DeleteBuff("Smoke_Buff", cell.occupant?.GetComponent<ChessBase>());
            Destroy(this.gameObject);
        }
    }
}
