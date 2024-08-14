using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Snipe : Card, IPointerDownHandler, IPointerUpHandler
{
    public int damage;
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
        FightUI.instance.ChangeLineColor(line,this);
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
        //获取当前鼠标所处点击的cell
        Debug.Log(ChessboardManager.instance.curCell.name);
        string selectedCell = ChessboardManager.instance.curCell.name;
        Vector2Int selectedCellPos = new Vector2Int(int.Parse(selectedCell[6].ToString()), int.Parse(selectedCell[8].ToString()));
        //如果当前cell没有敌人，弹回手牌、
        if (ChessboardManager.instance.CheckCell(selectedCellPos) == null || ChessboardManager.instance.CheckCell(selectedCellPos) == PlayerController.instance)
        {
            this.isUsed = false;
            Debug.Log("No enemy in this cell");
            return;
        }
        else
        {
            EnemyBase enemy = ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>();
            //计算敌人距离玩家的距离
            int xAbs= Mathf.Abs(enemy.location.x - PlayerController.instance.location.x);
            int yAbs = Mathf.Abs(enemy.location.y - PlayerController.instance.location.y);
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
            GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletParticle");
            GameObject HitEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletHitEffect");
            PlayerController.instance.BulletAttack(damage, enemy, BulletPrefab, HitEffect);
        }
        dragFlag = 0;isDrag = false;
        costManager.instance.curCost -= cost;
    }
}
