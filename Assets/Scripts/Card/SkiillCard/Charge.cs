using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charge : Card
{
    public new void Update()
    {
        //如果正在被拖拽，将该卡牌变透明
        if (isDrag)
        {
            this.GetComponent<Image>().color= new Color(0, 0, 0, 0.05f);
        }
        else
        {
            this.GetComponent<Image>().color = startColor;
        }
    }
    public override void CardFunc()
    {
        /*选择一个方向进行移动，直到与一个怪物进行碰撞或触碰到障碍。碰撞时对怪物造成基础近战伤害*/
        //获取当前鼠标所处点击的cell
        Debug.Log(ChessboardManager.instance.curCell.name);
        string selectedCell = ChessboardManager.instance.curCell.name;
        Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
        //Debug.Log("seletedCell is:" + selectedCellPos+ int.Parse(selectedCell[8].ToString()));
        //如果选择错误的方向，弹回手牌
        if (selectedCellPos.x != PlayerController.instance.Location.x && selectedCellPos.y != PlayerController.instance.Location.y)
        {
            this.isUsed = false;
            Debug.Log("Wrong direction");
            return;
        }
        if (selectedCellPos.x - PlayerController.instance.Location.x > 0)
        {
            PlayerController.instance.Move(new Vector2Int(10, 0));
        }
        else if (selectedCellPos.x - PlayerController.instance.Location.x < 0)
        {
            PlayerController.instance.Move(new Vector2Int(-10, 0));
        }
        else if (selectedCellPos.y - PlayerController.instance.Location.y > 0)
        {
            PlayerController.instance.Move(new Vector2Int(0, 10));
        }
        else if (selectedCellPos.y - PlayerController.instance.Location.y < 0)
        {
            PlayerController.instance.Move(new Vector2Int(0, -10));
        }
        costManager.instance.curCost -= cost;
    }
}
