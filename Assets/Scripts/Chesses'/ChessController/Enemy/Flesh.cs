using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flesh : EnemyBase
{//模糊血肉
    void Awake()
    {
        // 初始化敌人棋子
        MaxHp = 30;//最大生命值
        HP = 30;//当前生命值
        MeleeAttackPower = 8;//近战攻击力
        mobility = 1;//行动力
        moveMode = 3;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "模糊血肉";//棋子名称
        
        ChessboardManager.instance.AddChess(this.gameObject, Location);
        
    }

    public override IEnumerator OnTurn()
    {
        //该敌人回合

        //用BFS算法移动
        yield return base.OnTurn();

        yield return new WaitForSeconds(0.3f);
    }
}
