using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flesh : EnemyBase
{//模糊血肉
    void Start()
    {
        // 初始化敌人棋子
        MaxHp = 30;//最大生命值
        HP = 30;//当前生命值
        meleeAttackPower = 8;//近战攻击力
        mobility = 1;//行动力
        moveModel = 3;//移动模式
        chessboardManager.AddChess(this.gameObject, Location);
        this.gameObject.tag = "Enemy";
    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合

        //用BFS算法移动
        yield return base.OnTurn();

        yield return new WaitForSeconds(0.3f);
    }
}
