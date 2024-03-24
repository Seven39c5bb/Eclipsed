using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBase : ChessBase
{
    public int mobility;//行动力
    public int moveModel;//移动模式
    public virtual IEnumerator OnTurn()
    {
        //用BFS算法找到前往玩家的最短路径
        for (int i = 0; i < mobility; i++)
        {
            List<Vector2Int> path = chessboardManager.FindPath(Location, PlayerController.instance.Location, PlayerController.instance.gameObject, CellsInRange);
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

                Vector2Int nextDirection = (path[1] - Location) * moveModel;
                //在移动前，还需要判断目标位置是否玩家已经进入射程，怪物会与玩家保持一定距离
                /* Vector2Int nextLocation = Location + nextDirection;
                while (WillIsPlayerInRange(nextLocation))
                {
                    int norm = Mathf.Abs(nextDirection.x) + Mathf.Abs(nextDirection.y);
                    if (norm == 0)//如果无法移动
                    {
                        break;
                    }
                    nextDirection = nextDirection * (norm - 1) / norm;//缩短移动距离
                    nextLocation = Location + nextDirection;
                } */
                Debug.Log("nextDirection: " + nextDirection);
                Move(nextDirection);
                //等待nextDirection的模*0.5f的时间后，再继续循环
                float delay = 0.5f * (Mathf.Abs(nextDirection.x) + Mathf.Abs(nextDirection.y));
                yield return new WaitForSeconds(delay);
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
