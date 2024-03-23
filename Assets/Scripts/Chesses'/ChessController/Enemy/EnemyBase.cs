using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : ChessBase
{
    public int mobility;//行动力
    public int moveModel;//移动模式
    public virtual IEnumerator OnTurn()
    {
        yield return null;
    }
}
