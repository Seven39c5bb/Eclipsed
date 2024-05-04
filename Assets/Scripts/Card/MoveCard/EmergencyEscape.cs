using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmergencyEscape : Card, IPointerDownHandler, IPointerUpHandler
{
    GameObject line;
    public new void Update()
    {
        //如果正在被拖拽，将该卡牌变透明
        if (isDrag)
        {
            this.GetComponent<Image>().color = new Color(0, 0, 0, 0.05f);
        }
        else
        {
            this.GetComponent<Image>().color = startColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        line = Instantiate(Resources.Load("Prefabs/UI/LineUI"), GameObject.Find("Canvas").transform) as GameObject;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Destroy(line);
    }
    public override void CardFunc()
    {
        /*选择一个方向进行移动，直到与一个怪物进行碰撞或触碰到障碍。碰撞时对怪物造成基础近战伤害*/
        //获取当前鼠标所处点击的cell
        Debug.Log(ChessboardManager.instance.curCell.name);
        string selectedCell = ChessboardManager.instance.curCell.name;
        Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
        if(ChessboardManager.instance.CheckCell(selectedCellPos) != null)
        {
            isUsed = false;
            return;
        }
        PlayerController.instance.Teleport(selectedCellPos);
        BuffManager.instance.AddBuff("EmergencyEscapeBuff", PlayerController.instance);
    }
}
