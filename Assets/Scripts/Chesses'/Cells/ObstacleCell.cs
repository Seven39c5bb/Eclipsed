using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCell : Cell
{
    public override void Awake()
    {
        state = StateType.Wall;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //设置为黑色
        spriteRenderer.color = Color.gray;//墙体地块设置为黑色

        //填充墙体的名字和描述
        cellName = "障碍";
        cellDescription = "棋子无法通过";

        base.Awake();//记录当前颜色
    }

    public override void OnMouseEnter()
    {
        isSelected = true;
        ChessboardManager.instance.curCell = this;
    }

    public override void OnMouseExit()
    {
        ChessboardManager.instance.curCell = null;
        isSelected = false;
    }
}
