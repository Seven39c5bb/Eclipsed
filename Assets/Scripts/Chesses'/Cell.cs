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
    
    public bool isBloodPool = false;//是否是血池

    SpriteRenderer spriteRenderer;
    Color originColor;
    private void Awake()
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
        // 将颜色设置为透明
        originColor = spriteRenderer.color;
        spriteRenderer.color = new Color(0, 0, 0, 0.05f);
    }
    private void OnMouseExit()
    {
        ChessboardManager.instance.curCell = null;
        isSelected = false;
        // 将颜色设置为原始颜色
        spriteRenderer.color = originColor;
    }
}
