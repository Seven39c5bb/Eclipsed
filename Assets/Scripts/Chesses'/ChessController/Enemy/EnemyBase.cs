using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBase : ChessBase
{
    public int mobility;//行动力
    public int moveMode;//移动模式

    public bool isActed = false;

    public virtual IEnumerator OnTurn()
    {
        //用BFS算法找到前往玩家的最短路径
        int originMobility = mobility;//记录初始行动力，行动力可能会在移动过程中因碰撞或各种原因减少
        for (int i = 0; i < originMobility; i++)
        {

            List<Vector2Int> path = ChessboardManager.instance.FindPath(Location, PlayerController.instance.Location, PlayerController.instance.gameObject, CellsInRange);
            //向玩家附近移动

            if (path == null || path.Count == 0)//防止越界
            {
                Debug.LogError("Path is too short");
                yield break;
            }

            if(IsInRange(PlayerController.instance.Location))//如果处在玩家周围偏好圈内，直接释放技能（防止怪物在偏好区内来回移动）
            {
                break;
            }

            if(path.Count == 1)//如果已经到达偏好区，直接释放技能，防止下面访问path[1]时越界(path[0]是自己当前位置)
            {
                break;
            }
            else
            {

                Vector2Int nextDirection = (path[1] - Location) * moveMode;
                Debug.Log("nextDirection: " + nextDirection);
                (int residualDistance, bool isMeleeAttack, bool isRotate) = Move(nextDirection);
                float attackDelay = 0;
                if (isMeleeAttack)
                {
                    Debug.Log("Melee Attack Delay");
                    attackDelay = 1.11f;
                }
                float rotateDelay = 0;
                if (isRotate)
                {
                    Debug.Log("Rotate Delay");
                    rotateDelay = 0.5f;
                }
                //等待nextDirection的模*0.5f的时间后，再继续循环
                float delay = 0.5f * (Mathf.Abs(nextDirection.x) + Mathf.Abs(nextDirection.y) - residualDistance) + attackDelay + rotateDelay + 0.3f;
                yield return new WaitForSeconds(delay);

                // 在每次移动之后都检查怪物是否已经死亡
                if (HP <= 0)
                {
                    //结束协程
                    yield break;
                }
            }
        }
    }

    public virtual bool IsInRange(Vector2Int Location)//判断是否在该怪物偏好的环内
    {
        return false;//可直接用于碰撞类怪物,玩家位置既是偏好区
    }

    public virtual Vector2Int[] CellsInRange(Vector2Int Location)//获取玩家周围偏好区，Location为玩家的位置
    {
        //返回Location
        return new Vector2Int[] { Location };//可直接用于碰撞类怪物，玩家位置既是偏好区
    }
}
