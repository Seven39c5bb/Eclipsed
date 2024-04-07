using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : ChessBase
{
    public static PlayerController instance;
    public int initHp;
    public override void Start()
    {
        // 从存档中读取玩家初始生命值
        /* if (SaveManager.instance.jsonData.playerData.HP == 0)
        {
            initHp = 80;
        }
        else
        {
            initHp = SaveManager.instance.jsonData.playerData.HP;
        } */
        initHp = 80;

        // 初始化玩家棋子
        instance = this;
        MaxHp = 80;//最大生命值
        HP = initHp;//当前生命值
        MeleeAttackPower = 10;//近战攻击力
        chessName = "魔女";//棋子名称

        base.Start();//添加血条
        chessboardManager.AddChess(this.gameObject, Location);

        //将标签设置为玩家
        this.gameObject.tag = "Player";
    }
}
