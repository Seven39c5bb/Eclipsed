using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aoe : Card
{
    //以主角为中心，对主角周围3*3的敌人造成伤害
    public int damage;
    public override void CardFunc()
    {
        //获取主角位置
        Vector2Int playerPos = PlayerController.instance.Location;
        List<Vector2Int> posList = new List<Vector2Int>();
        //
        for(int i = -2; i <= 2; i++)
        {
            for(int j = -2; j <=2; j++)
            {
                posList.Add(new Vector2Int(playerPos.x+i, playerPos.y+j));
            }
        }
        posList.Remove(playerPos);
        Instantiate(Resources.Load<GameObject>("Prefabs/Particle/PlayerEffect/PlayerAOEEffect"), PlayerController.instance.transform.position, Quaternion.identity);
        foreach(var pos in posList) {
            if (pos.x < 0 || pos.y < 0 || pos.x > 9 || pos.y > 9) continue;
            Debug.Log(pos);
            if (ChessboardManager.instance.cellStates[pos.x, pos.y].state == Cell.StateType.Occupied) 
            {
                ChessboardManager.instance.CheckCell(pos).TakeDamage(damage, PlayerController.instance);
            }
        }
        costManager.instance.curCost -= cost;
    }
}
