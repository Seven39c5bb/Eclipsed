using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightInit : FightUnit
{
    public override void Init()
    {
        Debug.Log("this init fightunit init");
        //test:��Ӽ����ƽ�����
        //GameObject obj= Resources.Load("Prefabs/Card/up") as GameObject;
        //CardManager.instance.cardDesk.Add(obj.GetComponent<Card>());
        //��ʼ����ɺ��л�����һغ�
        FightManager.instance.ChangeType(FightType.Player);
    }
    public override void OnUpdate()
    {
        
    }
}
