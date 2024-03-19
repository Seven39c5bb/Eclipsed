using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightInit : FightUnit
{
    public override void Init()
    {
        Debug.Log("this init fightunit init");
        //test:添加几张牌进牌组
        //GameObject obj= Resources.Load("Prefabs/Card/up") as GameObject;
        //CardManager.instance.cardDesk.Add(obj.GetComponent<Card>());
        //初始化完成后切换到玩家回合
        FightManager.instance.ChangeType(FightType.Player);
    }
    public override void OnUpdate()
    {
        
    }
}
