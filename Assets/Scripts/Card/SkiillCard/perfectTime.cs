using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class perfectTime :Card
{
    public int damage;
    public override void CardFunc()
    {
        Vector2Int playerPos = PlayerController.instance.Location;
        //如果玩家前方四格，向上三格子有敌人，对其造成伤害
        if(ChessboardManager.instance.cellStates[playerPos.x+4, playerPos.y-3].state == Cell.StateType.Occupied)
        {
            Vector2Int pos = new Vector2Int(playerPos.x + 4, playerPos.y - 3);
            ChessboardManager.instance.CheckCell(pos).TakeDamage(damage);
        }
        costManager.instance.curCost -= cost;
    }
}
