using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Candelabra_Buff : BuffBase
{
    public int flag = 0;
    void Awake()
    {
        buffName = "Candelabra_Buff";
        buffNameCN = "全神贯注";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        description = "照亮周围";
        canBeLayed = true;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Action;
    }
    public override int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        //改变烛台颜色
        flag = flag == 0 ? 1 : 0;
        if(flag==0)
        {
            chessBase.GetComponent<SpriteRenderer>().material.color = Color.white;
        }
        else
        {
            chessBase.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        }
        //照亮周围5*5的地块
        int leftAxis = chessBase.location.x - 2 > 0 ? chessBase.location.x - 2 : 0;
        int rightAxis = chessBase.location.x + 2 < 9 ? chessBase.location.x + 2 : 9;
        int upAxis = chessBase.location.y - 2 > 0 ? chessBase.location.y - 2 : 0;
        int downAxis = chessBase.location.y + 2 < 9 ? chessBase.location.y + 2 : 9;
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                if (ChessboardManager.instance.cellStates[i, j].property?.propertyName =="Smoke" )
                {
                    ChessboardManager.instance.ChangeProperty(new Vector2Int(i,j),null);
                    chessBase.GetComponent<Candelabra>()?.points.Add(new Vector2Int(i,j));
                    Debug.Log(i +" "+ j + " 上有烟雾");
                }
            }
        }
        return 0;
    }

    public override void OnPlayerTurnBegin()
    {
        base.OnPlayerTurnBegin();
    }
}
