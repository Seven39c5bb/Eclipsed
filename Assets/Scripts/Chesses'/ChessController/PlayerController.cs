using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : ChessBase
{
    public static PlayerController instance;
    public override void Start()
    {
        // 初始化玩家棋子
        instance = this;
        MaxHp = 80;//最大生命值
        HP = 80;//当前生命值
        meleeAttackPower = 10;//近战攻击力

        base.Start();//添加血条
        chessboardManager.AddChess(this.gameObject, Location);

        //将标签设置为玩家
        this.gameObject.tag = "Player";
    }
}
