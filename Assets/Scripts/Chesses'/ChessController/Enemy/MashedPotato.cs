using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashedPotato : EnemyBase
{//土豆泥
    void Awake()
    {
        // 初始化敌人棋子
        MaxHp = 50;//最大生命值
        HP = 50;//当前生命值
        MeleeAttackPower = 13;//近战攻击力
        mobility = 2;//行动力
        moveMode = 1;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "土豆泥";//棋子名称

        ChessboardManager.instance.AddChess(this.gameObject, Location);
        
    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合
        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnStart();
        }

        //用BFS算法移动
        yield return base.OnTurn();

        yield return new WaitForSeconds(0.3f);

        foreach (BuffBase buff in buffList)
        {
            buff.OnTurnEnd();
        }
    }
    
}
