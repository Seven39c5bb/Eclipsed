using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class evilflame : Card
{
    int addFlag = 0;
    public new void Update()
    {
        base.Update();
        if(addFlag==0)
        {
            BuffManager.instance.AddBuff("evilflameBuff", PlayerController.instance);
            addFlag = 1;
            Debug.Log("evilflame");
        }
    }
    public override void CardFunc()
    {
        BuffManager.instance.DeleteBuff("evilflameBuff", PlayerController.instance);
        costManager.instance.curCost -= cost;
    }
}
