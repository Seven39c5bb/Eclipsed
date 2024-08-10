using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public  enum StateType
    {
        Empty,
        Wall,
        Occupied
    }

    public StateType state = StateType.Empty;
    public GameObject occupant = null;
    //是否被鼠标选中
    public bool isSelected = false;
    //该棋格上的属性
    public CellProperty property;
    //该棋格的位置
    public Vector2Int cellLocation;

    //棋格地形的名字和描述
    public string cellName;
    public string cellDescription;


    public SpriteRenderer spriteRenderer;
    public Color originColor;
    public virtual void Awake()
    {
        if (occupant == null && state != StateType.Wall)
        {
            state = StateType.Empty;
        }
        //记录当前颜色
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    public virtual void OnMouseEnter()
    {
        isSelected = true;
        ChessboardManager.instance.curCell = this;
        
        // 将颜色设置为黄色
        originColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 0, 1);
    }
    public virtual void OnMouseExit()
    {
        ChessboardManager.instance.curCell = null;
        isSelected = false;

        // 将颜色设置为原始颜色
        spriteRenderer.color = originColor;
    }

    #region
    public virtual void OnAdd()
    {

    }
    public virtual void OnChessEnter(ChessBase chess)
    {

    }
    public virtual void OnChessExit(ChessBase chess)
    {

    }
    public virtual void OnChessReach(ChessBase chess)//棋子移动结束到达
    {

    }
    public virtual void OnChessDepart(ChessBase chess)//棋子从此出发
    {

    }

    public virtual void OnPlayerTurnBegin()
    {

    }
    #endregion
}
