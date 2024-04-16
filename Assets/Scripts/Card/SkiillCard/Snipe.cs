using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : Card
{
    public int damage;
    public new void Update()
    {
        //如果正在被拖拽，将该卡牌变透明
        if (isDrag)
        {
            this.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0.05f);
        }
        else
        {
            this.GetComponent<UnityEngine.UI.Image>().color = startColor;
        }
    }
    public override void CardFunc()
    {
        //获取当前鼠标所处点击的cell
        Debug.Log(ChessboardManager.instance.curCell.name);
        string selectedCell = ChessboardManager.instance.curCell.name;
        Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
        //如果当前cell没有敌人，弹回手牌、
        if (ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>() == null)
        {
            this.isUsed = false;
            Debug.Log("No enemy in this cell");
            return;
        }
        else
        {
            EnemyBase enemy = ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>();
            //计算敌人距离玩家的距离
            int xAbs= Mathf.Abs(enemy.Location.x - PlayerController.instance.Location.x);
            int yAbs = Mathf.Abs(enemy.Location.y - PlayerController.instance.Location.y);
            //取xabs和yabs中的最大值
            int maxAbs = xAbs > yAbs ? xAbs : yAbs;
            //如果距离大于等于3，伤害+4、
            if (maxAbs >= 3)
            {
                damage += 4;
            }
            //如果距离大于等于5，伤害+8
            if (maxAbs >= 5)
            {
                damage += 8;
            }
            enemy.TakeDamage(damage, PlayerController.instance);
        }
        costManager.instance.curCost -= cost;
    }
}
