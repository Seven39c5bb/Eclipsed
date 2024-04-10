using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFleshLoad_BloodSoup : BuffBase
{
    void Awake()
    {
        buffName = "BuffFleshLoad_BloodSoup";
        buffNameCN = "血肉上膛";
        description = "血肉已经上膛，准备发射！";
        durationTurn = 9999;
    }
    public Vector2Int playerPrePos;
    public GameObject BloodSoup_AimBar;
    public override void OnTurnStart()
    {
        Debug.Log("血肉发射！");
        Destroy(BloodSoup_AimBar);
        //对playerPrePos为中心3*3的区域内的玩家单位进行伤害12, 并将对应棋格cellCondition设置为BloodPool_Shallow
        for (int i = playerPrePos.x - 1; i <= playerPrePos.x + 1; i++)
        {
            for (int j = playerPrePos.y - 1; j <= playerPrePos.y + 1; j++)
            {
                if (i >= 0 && i < ChessboardManager.instance.cellStates.GetLength(0) && j >= 0 && j < ChessboardManager.instance.cellStates.GetLength(1))
                {
                    if (ChessboardManager.instance.cellStates[i, j].state == Cell.StateType.Occupied && ChessboardManager.instance.cellStates[i, j].occupant.tag == "Player")
                    {
                        ChessboardManager.instance.cellStates[i, j].occupant.GetComponent<PlayerController>().TakeDamage(12, BloodSoup.Instance);
                    }
                    if (ChessboardManager.instance.cellStates[i, j].cellCondition != Cell.CellCondition.BloodPool_Deep)//如果不是深水，就设置为浅水
                    {
                        ChessboardManager.instance.cellStates[i, j].SetBloodPool(Cell.CellCondition.BloodPool_Shallow);
                    }
                }
            }
        }
        BuffManager.instance.DeleteBuff(buffName, chessBase);
    }
    public override void OnAdd()
    {
        playerPrePos = PlayerController.instance.Location;
        //在玩家位置所在棋格生成一个目标指示器
        BloodSoup_AimBar = Instantiate(Resources.Load<GameObject>("Prefabs/BloodSoup_AimBar"));
        var position = ChessboardManager.instance.cellStates[playerPrePos.x, playerPrePos.y].transform.position;
        position.z = BloodSoup_AimBar.transform.position.z; // 保持原始的 z 值
        BloodSoup_AimBar.transform.position = position;
        BloodSoup_AimBar.transform.SetParent(ChessboardManager.instance.cellStates[playerPrePos.x, playerPrePos.y].transform, true);
    }
}
