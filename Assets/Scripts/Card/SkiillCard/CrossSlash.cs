using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSlash : Card
{
    //��5*5ʮ�ָ����ڵĵ�����ɻ������ս�˺���ֵ���˺�
    public override void CardFunc()
    {
        //��ȡ��ǰλ��
        Vector2Int playerPos = PlayerController.instance.Location;
        //��ȡʮ�ָ����ڵĵ���
        List<Vector2Int> posList = new List<Vector2Int>();
        for(int i=-5; i<=5; i++)
        {
            posList.Add(new Vector2Int(playerPos.x + i, playerPos.y));
            posList.Add(new Vector2Int(playerPos.x, playerPos.y + i));
        }
        posList.Remove(playerPos);
        foreach (var pos in posList)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x > 9 || pos.y > 9) continue;
            Debug.Log(pos);
            if (ChessboardManager.instance.cellStates[pos.x, pos.y].state == Cell.StateType.Occupied)
            {
                ChessboardManager.instance.CheckCell(pos).TakeDamage(PlayerController.instance.meleeAttackPower);
            }
        }
        costManager.instance.curCost -= cost;
    }
}
