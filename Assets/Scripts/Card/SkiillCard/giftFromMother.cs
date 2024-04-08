using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giftFromMother : Card
{
    public int damage;
    public override void CardFunc()
    {
        //�����3����������˺�
        for (int i = 0; i < 3; i++)
        {
            ChessboardManager.instance.enemyControllerList[Random.Range(0, ChessboardManager.instance.enemyControllerList.Count)].TakeDamage(damage, PlayerController.instance);
        }
        costManager.instance.curCost -= cost;
    }
}
