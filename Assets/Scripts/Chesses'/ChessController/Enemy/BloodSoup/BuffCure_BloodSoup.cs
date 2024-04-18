using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCure_BloodSoup : BuffBase
{
    void Awake()
    {
        buffName = "BuffCure_BloodSoup";
        buffNameCN = "血肉愈合";
        description = "血羹正在缓慢恢复生命";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.HP;
    }
    public override void OnTurnStart()
    {
        Debug.Log("血肉愈合！");
        //对血羹进行治疗
        BloodSoup.Instance.Cure(8);
    }
}
