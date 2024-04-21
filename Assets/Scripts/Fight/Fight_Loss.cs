using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Fight_Loss : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.ShowUI<LossContinueUI>("LossPanel");
    }
    public override void OnUpdate()
    {

    }
}
