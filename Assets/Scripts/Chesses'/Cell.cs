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


    SpriteRenderer spriteRenderer;
    public Color originColor;
    public virtual void Awake()
    {
        if (state == StateType.Wall)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //设置为黑色
            spriteRenderer.color = Color.gray;
        }
        //记录当前颜色
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    private void OnMouseEnter()
    {
        isSelected = true;
        ChessboardManager.instance.curCell = this;
        // 将颜色设置为黄色
        if (state != StateType.Wall)
        {
            originColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1, 1, 0, 1);
        }
    }
    private void OnMouseExit()
    {
        ChessboardManager.instance.curCell = null;
        isSelected = false;
        if (state != StateType.Wall)
        {
            // 将颜色设置为原始颜色
            spriteRenderer.color = originColor;
        }
    }

    #region
    public virtual void OnAdd()
    {

    }
    public virtual void OnPlayerEnter()
    {

    }
    public virtual void OnPlayerExit()
    {

    }
    public virtual void OnPlayerTurnBegin()
    {

    }
    #endregion
}
