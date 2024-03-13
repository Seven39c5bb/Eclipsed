using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInit : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.HideUI("FightUI");
        Debug.Log("this init fightunit init");
    }
    public override void OnUpdate()
    {
        
    }
}
