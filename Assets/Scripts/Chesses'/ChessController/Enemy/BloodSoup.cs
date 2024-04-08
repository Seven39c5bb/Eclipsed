using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSoup : EnemyBase
{
    public new void Start()
    {
        // 初始化敌人棋子
        MaxHp = 120;//最大生命值
        HP = 120;//当前生命值
        MeleeAttackPower = 0;//近战攻击力
        mobility = 1;//行动力
        moveMode = 2;//移动模式
        this.gameObject.tag = "Enemy";
        chessName = "血汤";//棋子名称

        base.Start();//添加血条
        chessboardManager.AddChess(this.gameObject, Location);
    }
}
