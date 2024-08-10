using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Charge : Card,IPointerDownHandler,IPointerUpHandler
{
    GameObject line;
    int dragFlag = 1;
    public new void Update()
    {
        //如果正在被拖拽，将该卡牌变透明
        if (isDrag)
        {
            this.GetComponent<CanvasGroup>().alpha = 0.1f;
        }
        else
        {
            if(dragFlag == 1)
            this.GetComponent<CanvasGroup>().alpha = 1f;
        }

        #region 让线变色
        if (ChessboardManager.instance.curCell != null)
        {
            string selectedCell = ChessboardManager.instance.curCell.name;
            Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
            if ((selectedCellPos.x != PlayerController.instance.location.x && selectedCellPos.y != PlayerController.instance.location.y) || selectedCellPos == PlayerController.instance.location)
            {
                if (line != null)
                {
                    for (int i = 0; i < line.transform.childCount; i++)
                    {
                        line.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                    }
                }
            }
            else
            {
                if (line != null)
                {
                    for (int i = 0; i < line.transform.childCount; i++)
                    {
                        line.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
                    }
                }
            }
        }
        #endregion
    }
    //按下时生成一条线
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
        //Debug.Log("seletedCell is:" + selectedCellPos+ int.Parse(selectedCell[8].ToString()));
        //如果选择错误的方向，弹回手牌
        if ((selectedCellPos.x != PlayerController.instance.location.x && selectedCellPos.y != PlayerController.instance.location.y) || selectedCellPos == PlayerController.instance.location)
        {
            this.isUsed = false;
            Debug.Log("Wrong direction");
            return;
        }
        
        if (selectedCellPos.x - PlayerController.instance.location.x > 0)
        {
            Vector2Int aimDirection = new Vector2Int(0, 0);
            Vector2Int aimPos = new Vector2Int(0, 0);
            for (int i = 1; i <= 10; i++)
            {
                if (PlayerController.instance.location.x + i > 9)
                {
                    aimPos = new Vector2Int(9, PlayerController.instance.location.y);
                    break;
                }
                if (ChessboardManager.instance.CheckCell(new Vector2Int(PlayerController.instance.location.x + i, PlayerController.instance.location.y)) != null)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x + i, PlayerController.instance.location.y);
                    break;
                }
            }
            aimDirection = aimPos - PlayerController.instance.location;
            //PlayerController.instance.Move(aimDirection);
            StartCoroutine(PlayerController.instance.Move(aimDirection));
        }
        else if (selectedCellPos.x - PlayerController.instance.location.x < 0)
        {
            Vector2Int aimDirection = new Vector2Int(0, 0);
            Vector2Int aimPos = new Vector2Int(0, 0);
            for (int i = 1; i <= 10; i++)
            {
                if (PlayerController.instance.location.x - i < 0)
                {
                    aimPos = new Vector2Int(0, PlayerController.instance.location.y);
                    break;
                }
                if (ChessboardManager.instance.CheckCell(new Vector2Int(PlayerController.instance.location.x - i, PlayerController.instance.location.y)) != null)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x - i, PlayerController.instance.location.y);
                    break;
                }
            }
            aimDirection = aimPos - PlayerController.instance.location;
            //PlayerController.instance.Move(aimDirection);
            StartCoroutine(PlayerController.instance.Move(aimDirection));
        }
        else if (selectedCellPos.y - PlayerController.instance.location.y > 0)
        {
            Vector2Int aimDirection = new Vector2Int(0, 0);
            Vector2Int aimPos = new Vector2Int(0, 0);
            for (int i = 1; i <= 10; i++)
            {
                if (PlayerController.instance.location.y + i > 9)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x, 9);
                    break;
                }
                if (ChessboardManager.instance.CheckCell(new Vector2Int(PlayerController.instance.location.x, PlayerController.instance.location.y + i)) != null)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x, PlayerController.instance.location.y + i);
                    break;
                }
            }
            aimDirection = aimPos - PlayerController.instance.location;
            //PlayerController.instance.Move(aimDirection);
            StartCoroutine(PlayerController.instance.Move(aimDirection));
        }
        else if (selectedCellPos.y - PlayerController.instance.location.y < 0)
        {
            Vector2Int aimDirection = new Vector2Int(0, 0);
            Vector2Int aimPos = new Vector2Int(0, 0);
            for (int i = 1; i <= 10; i++)
            {
                if (PlayerController.instance.location.y - i < 0)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x, 0);
                    break;
                }
                if (ChessboardManager.instance.CheckCell(new Vector2Int(PlayerController.instance.location.x, PlayerController.instance.location.y - i)) != null)
                {
                    aimPos = new Vector2Int(PlayerController.instance.location.x, PlayerController.instance.location.y - i);
                    break;
                }
            }
            aimDirection = aimPos - PlayerController.instance.location;
            //PlayerController.instance.Move(aimDirection);
            StartCoroutine(PlayerController.instance.Move(aimDirection));
        }
        dragFlag = 0;isDrag = false;//使用卡牌以后将卡牌完全变透明
        costManager.instance.curCost -= cost;
    }


}
