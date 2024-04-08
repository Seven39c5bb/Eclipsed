using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerController : ChessBase
{
    public static PlayerController player_instance;
    public static PlayerController instance
    {
        get
        {
            if (player_instance == null)
            {
                player_instance = GameObject.FindObjectOfType<PlayerController>();
                if (SaveManager.instance.jsonData.playerData.HP == 0)
                {
                    player_instance.initHp = 80;
                }
                else
                {
                    player_instance.initHp = SaveManager.instance.jsonData.playerData.HP;
                }
                player_instance.Hp = player_instance.initHp;
            }
            return player_instance;
        }
    }
    public int initHp;
    public int coins;
    public int fingerBones;
    public override void Start()
    {
        // 从存档中读取玩家初始生命值
        if (SaveManager.instance.jsonData.playerData.HP == 0)
        {
            initHp = 80;
        }
        else
        {
            initHp = SaveManager.instance.jsonData.playerData.HP;
        }
        //initHp = 80;

        // 初始化玩家棋子
        player_instance = this;
        MaxHp = 80;//最大生命值
        HP = initHp;//当前生命值
        MeleeAttackPower = 10;//近战攻击力
        chessName = "魔女";//棋子名称

        base.Start();//添加血条
        ChessboardManager.instance.AddChess(this.gameObject, Location);

        //将标签设置为玩家
        this.gameObject.tag = "Player";
    }
}
