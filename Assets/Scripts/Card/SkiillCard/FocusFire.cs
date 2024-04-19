using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusFire : Card
{

    public int damage = 5;
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
        //对5格外的一个敌人造成伤害
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
            //判断敌人是否在5格内

            EnemyBase enemy = ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>();
            if(Mathf.Abs(enemy.Location.x - PlayerController.instance.Location.x) >= 5 
                || Mathf.Abs(enemy.Location.y - PlayerController.instance.Location.y )>= 5)
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
        costManager.instance.curCost -= cost;
    }
}
