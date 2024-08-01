using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FocusFire : Card, IPointerDownHandler, IPointerUpHandler
{

    public int damage = 5;
    GameObject line;int dragFlag = 1;
    public new void Update()
    {
        //如果正在被拖拽，将该卡牌变透明
        if (isDrag)
        {
            this.GetComponent<CanvasGroup>().alpha = 0.1f;
        }
        else
        {
            if(dragFlag==1)
            this.GetComponent<CanvasGroup>().alpha = 1f;
        }
        #region 让线变色
        if (ChessboardManager.instance.curCell != null)
        {
            string selectedCell = ChessboardManager.instance.curCell.name;
            Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
            if (ChessboardManager.instance.CheckCell(selectedCellPos))
            {
                if (ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>() != null
                    && (Mathf.Abs(ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>().location.x - PlayerController.instance.location.x) >= 2
                || Mathf.Abs(ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>().location.y - PlayerController.instance.location.y) >= 2))
                {
                    if (line != null)
                    {
                        for (int i = 0; i < line.transform.childCount; i++)
                        {
                            line.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }
                else
                {
                    if (line != null)
                    {
                        for (int i = 0; i < line.transform.childCount; i++)
                        {
                            line.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
            else
            {
                if (line != null)
                {
                    for (int i = 0; i < line.transform.childCount; i++)
                    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                        line.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                    }
                }
            }
        }
        #endregion
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
        //如果player中有FocusFireBuff，那么就不再添加
        bool hasBuff = false;
        foreach (BuffBase buff in PlayerController.instance.buffList)
        {
            if (buff.buffName == "FocusFireBuff")
            {
                hasBuff = true;
                break;
            }
        }
        if(!hasBuff)
        {
            BuffManager.instance.AddBuff("FocusFireBuff",PlayerController.instance);
        }
        else
        {
            //如果已经有FocusFireBuff，那么就增加伤害
            foreach (BuffBase buff in PlayerController.instance.buffList)
            {
                if (buff.buffName == "FocusFireBuff")
                {
                    buff.GetComponent<FocusFireBuff>().buffDamage += 5;
                    damage += buff.GetComponent<FocusFireBuff>().buffDamage;
                }
            }
        }
        //对2格外的一个敌人造成伤害
        //获取当前鼠标所处点击的cell
        Debug.Log(ChessboardManager.instance.curCell.name);
        string selectedCell = ChessboardManager.instance.curCell.name;
        Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
        //如果selectedCellPos上没有怪物，弹回手牌
        if (ChessboardManager.instance.CheckCell(selectedCellPos)==null || ChessboardManager.instance.CheckCell(selectedCellPos)==PlayerController.instance)
        {
            this.isUsed = false;
            Debug.Log("No enemy in this cell");
            return;
        }
        else
        {
            //判断敌人是否在2格内

            EnemyBase enemy = ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>();
            if(Mathf.Abs(enemy.location.x - PlayerController.instance.location.x) >= 2 
                || Mathf.Abs(enemy.location.y - PlayerController.instance.location.y )>= 2)
            {
                enemy.TakeDamage(damage, PlayerController.instance);
            }
            else
            {               
                this.isUsed = false;
                Debug.Log("Enemy is too near");
                return;
            }
        }
        dragFlag = 0;isDrag = false;
        costManager.instance.curCost -= cost;
    }
}
