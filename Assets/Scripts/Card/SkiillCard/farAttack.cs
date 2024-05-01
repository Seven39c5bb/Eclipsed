using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class farAttack : Card
{
    public int damage;
    public override void CardFunc()
    {
        Vector2Int playerPos = PlayerController.instance.Location;
        List<Vector2Int> posList = new List<Vector2Int>();
        //遍历玩家+-5格的所有位置
        for (int i = -5; i <= 5; i++)
        {
            for (int j = -5; j <= 5; j++)
            {
                posList.Add(new Vector2Int(playerPos.x + i, playerPos.y + j));
            }
        }
        Vector2Int enemyPos = new Vector2Int(-111,-111);
        posList.Remove(playerPos);
        float minDistance = 999999f;
        foreach (var pos in posList)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x > 9 || pos.y > 9) continue;
            //找到距离player最短距离的一个敌人位置
            if (ChessboardManager.instance.cellStates[pos.x, pos.y].state == Cell.StateType.Occupied)
            {
                float distance = Mathf.Sqrt(Mathf.Abs(playerPos.x - pos.x) * Mathf.Abs(playerPos.x - pos.x) +
                    Mathf.Abs(playerPos.y - pos.y) * Mathf.Abs(playerPos.y - pos.y));
                if(distance <= minDistance)
                {
                    minDistance = distance;
                    enemyPos = pos;
                }
                Debug.Log(enemyPos+"distance:"+distance);
            }
        }
        if(enemyPos!=new Vector2Int(-111,-111))
        {
            Debug.Log(enemyPos);
            GameObject BulletPrefab = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/PlayerBulletParticle");
            GameObject HitEffect = Resources.Load<GameObject>("Prefabs/Particle/PlayerBulletParticle/Hit Effect");
            //StartCoroutine(PlayerController.instance.BulletAttack(damage, ChessboardManager.instance.CheckCell(enemyPos), BulletPrefab, HitEffect));
            PlayerController.instance.BulletAttack(damage, ChessboardManager.instance.CheckCell(enemyPos), BulletPrefab, HitEffect);
        }
        else
        {
            isUsed = false;
            return;
        }
        costManager.instance.curCost -= cost;
    }
}
