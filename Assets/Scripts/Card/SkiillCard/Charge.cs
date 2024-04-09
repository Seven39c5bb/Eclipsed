using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Card
{
    public override void CardFunc()
    {
        /*选择一个方向进行移动，直到与一个怪物进行碰撞或触碰到障碍。碰撞时对怪物造成基础近战伤害*/
        Vector2Int direction = new Vector2Int(1, 0);

    }
}
