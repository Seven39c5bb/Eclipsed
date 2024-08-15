using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Priest_Buff : BuffBase
{
    public override void OnDie()
    {

        int leftAxis = chessBase.location.x - 1 > 0 ? chessBase.location.x - 1 : 0;
        int rightAxis = chessBase.location.x + 1 < 9 ? chessBase.location.x + 1 : 9;
        int upAxis = chessBase.location.y - 1 > 0 ? chessBase.location.y - 1 : 0;
        int downAxis = chessBase.location.y + 1 < 9 ? chessBase.location.y + 1 : 9;
        //yield return new WaitForSeconds(1f);
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessboardManager.instance.Summon(new Vector2Int(i, j), "Flesh");
            }
        }
        //StartCoroutine(SummonFleshes());
    }
    IEnumerator SummonFleshes()
    {
        
        int leftAxis = chessBase.location.x - 1 > 0 ? chessBase.location.x - 1 : 0;
        int rightAxis = chessBase.location.x + 1 < 9 ? chessBase.location.x + 1 : 9;
        int upAxis = chessBase.location.y - 1 > 0 ? chessBase.location.y - 1 : 0;
        int downAxis = chessBase.location.y + 1 < 9 ? chessBase.location.y + 1 : 9;
        yield return new WaitForSeconds(1f);
        for (int i = leftAxis; i <= rightAxis; i++)
        {
            for (int j = upAxis; j <= downAxis; j++)
            {
                ChessboardManager.instance.Summon(new Vector2Int(i, j), "Flesh");
            }
        }
    }
}
