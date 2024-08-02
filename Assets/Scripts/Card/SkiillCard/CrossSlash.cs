using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSlash : Card
{
    //对5*5十字格子内的敌人造成基于你近战伤害数值的伤害
    public override void CardFunc()
    {
        //获取当前位置
        Vector2Int playerPos = PlayerController.instance.location;
        //获取十字格子内的敌人
        List<Vector2Int> posList = new List<Vector2Int>();
        for(int i=-5; i<=5; i++)
        {
            posList.Add(new Vector2Int(playerPos.x + i, playerPos.y));
            posList.Add(new Vector2Int(playerPos.x, playerPos.y + i));
        }
        posList.Remove(playerPos);
        //十字斩特效
        GameObject CrossSlashEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerEffect/CrossSlashEffect");
        Instantiate(CrossSlashEffect, PlayerController.instance.gameObject.transform.position, Quaternion.identity);
        StartCoroutine(DelayedDamage(posList));
        costManager.instance.curCost -= cost;
    }

    IEnumerator DelayedDamage(List<Vector2Int> posList)
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var pos in posList)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x > 9 || pos.y > 9) continue;
            Debug.Log(pos);
            if (ChessboardManager.instance.cellStates[pos.x, pos.y].state == Cell.StateType.Occupied)
            {
                if (ChessboardManager.instance.CheckCell(pos).GetComponent<EnemyBase>() != null)
                ChessboardManager.instance.CheckCell(pos).TakeDamage(PlayerController.instance.meleeAttackPower_private,PlayerController.instance);
            }
        }
    }
}
