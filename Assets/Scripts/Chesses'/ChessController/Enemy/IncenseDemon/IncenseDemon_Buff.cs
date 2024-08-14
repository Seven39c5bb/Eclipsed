using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class IncenseDemon_Buff : BuffBase
{
    private void Awake()
    {
        buffName = "IncenseDemon_Buff";
        buffNameCN = "潜行";
        durationTurn = 10;
        buffType = BuffType.Buff;
        description = "";
        canBeLayed = false;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    public override void OnTurnStart()
    {
        Debug.Log("触发效果");
        //被动1：用烟雾笼罩自身。只有进入BOSS的5*5范围内时，才能对其造成伤害。
        //检测以自身为中心5*5范围内有没有玩家
        int hasPlayer = 0;
        int leftAxis = chessBase.location.x - 2 > 0 ? chessBase.location.x - 2 : 0;
        int rightAxis = chessBase.location.x + 2 < 9 ? chessBase.location.x + 2 : 9;
        int upAxis = chessBase.location.y - 2 > 0 ? chessBase.location.y - 2 : 0;
        int downAxis = chessBase.location.y + 2 < 9 ? chessBase.location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {               
                if (ChessboardManager.instance.cellStates[i,j].state==Cell.StateType.Occupied &&
                    ChessboardManager.instance.cellStates[i, j].occupant?.tag=="Player")
                {
                    Debug.Log("i:" + i + "j:" + j);
                    hasPlayer = 1;
                }
            }
        }
        Debug.Log("hasplayer:" + hasPlayer);
        //如果没有玩家
        if(hasPlayer == 0)
        {
            Debug.Log("没有玩家");
            for (int i = leftAxis; i <= rightAxis; i++)
            {
                for (int j = upAxis; j <= downAxis; j++)
                {
                    ChessboardManager.instance.ChangeProperty(new Vector2Int(i, j), "Smoke");
                }
            }
        }
    }
    public override void OnTurnEnd()
    {
        //检测以自身为中心5*5范围内有没有玩家
        int hasPlayer = 0;
        int leftAxis = chessBase.location.x - 2 > 0 ? chessBase.location.x - 2 : 0;
        int rightAxis = chessBase.location.x + 2 < 9 ? chessBase.location.x + 2 : 9;
        int upAxis = chessBase.location.y - 2 > 0 ? chessBase.location.y - 2 : 0;
        int downAxis = chessBase.location.y + 2 < 9 ? chessBase.location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                if (ChessboardManager.instance.cellStates[i, j].state == Cell.StateType.Occupied &&
                    ChessboardManager.instance.cellStates[i, j].occupant?.tag == "Player")
                {
                    Debug.Log("i:" + i + "j:" + j);
                    hasPlayer = 1;
                }
            }
        }
        //如果有玩家，则消除自身该格内的烟雾
        if (hasPlayer==1)
        {
            ChessboardManager.instance.ChangeProperty(chessBase.location, null);
        }
    }
    public override void OnPlayerReach()
    {
        //清除以自身5*5范围内的烟雾
        int leftAxis = chessBase.location.x - 2 > 0 ? chessBase.location.x - 2 : 0;
        int rightAxis = chessBase.location.x + 2 < 9 ? chessBase.location.x + 2 : 9;
        int upAxis = chessBase.location.y - 2 > 0 ? chessBase.location.y - 2 : 0;
        int downAxis = chessBase.location.y + 2 < 9 ? chessBase.location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessboardManager.instance.ChangeProperty(new Vector2Int(i, j), null);
            }
        }
    }

}
