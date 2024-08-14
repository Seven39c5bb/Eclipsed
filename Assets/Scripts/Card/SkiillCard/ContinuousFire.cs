using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinuousFire : Card, IPointerDownHandler, IPointerUpHandler
{
    public int damage;
    public GameObject line;bool isChangeColor = false;
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
        if (ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>() == null)
        {
            this.isUsed = false;
            Debug.Log("No enemy in this cell");
            return;
        }
        else
        {
            EnemyBase enemy = ChessboardManager.instance.CheckCell(selectedCellPos).GetComponent<EnemyBase>();
            int flag = 0;//标记是否击杀敌人
            if (damage >= enemy.HP)
            {
                flag = 1;
            }
            GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletParticle");
            GameObject HitEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletHitEffect");
            PlayerController.instance.BulletAttack(damage, enemy, BulletPrefab, HitEffect);
            if (flag==1)
            {
                CardManager.instance.Draw(1);
            }
        }
        dragFlag = 0;isDrag=false;
        costManager.instance.curCost -= cost;
    }
}
