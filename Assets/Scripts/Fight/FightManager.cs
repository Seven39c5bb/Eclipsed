using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FightType
{
    None,Init,Player,Enemy,Win,Loss
}
public class FightManager : MonoBehaviour
{
    private static FightManager f_instance;

    public static FightManager instance
    {
        get
        {
            if(f_instance == null)
            {
                f_instance=GameObject.FindObjectOfType<FightManager>();
            }
            return f_instance;
        }
    }
    public FightUnit fightUnit;
    public FightType curFightType;
    private void Awake()
    {
        f_instance = this;
    }
    public bool isCheckingVictory = false;//是否正在检查胜利(会在战斗初始化时设为true)
    private void Update()
    {
        if(fightUnit != null)
        {
            fightUnit.OnUpdate();
        }
        if (isCheckingVictory)
        {
            if (ChessboardManager.instance.enemyList.Count == 0)
            {
                ChangeType(FightType.Win);
                isCheckingVictory = false;
            }
        }
    }
    public void ChangeType(FightType type)
    {
        switch (type)
        {
            case FightType.None:
                break; 
            case FightType.Init:
                fightUnit=new FightInit();
                curFightType = FightType.Init;
                break;
            case FightType.Player:
                fightUnit=new Fight_PlayerTurn();
                curFightType=FightType.Player;
                break;
            case FightType.Enemy:
                fightUnit=new Fight_EnemyTurn();
                curFightType = FightType.Enemy;
                break;
            case FightType.Win:
                fightUnit = new Fight_win();
                curFightType=FightType.Win;
                break;
            case FightType.Loss:
                fightUnit=new Fight_Loss();
                curFightType = FightType.Loss;
                break;
        }
        fightUnit.Init();//初始化该战斗单元
    }
}
