using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPool : CellProperty
{
    public enum BloodPoolDepth
    {
        Shallow,
        Deep,
        Normal
    }
    public BloodPoolDepth bloodPoolDepth = BloodPoolDepth.Shallow;//血池深度
    public int sustainTurn = 0;//持续回合
    private GameObject bloodPool;//血池图像

    void Awake()
    {
        this.propertyName = "BloodPool";
        this.description = "血池";
    }

    public override void OnPlayerTurnBegin()
    {
        switch (bloodPoolDepth)
        {
            case BloodPoolDepth.Shallow:
                if (cell.state == Cell.StateType.Occupied && cell.occupant.tag == "Player")
                {
                    BloodSoup.instance.OnBloodPool(BloodPoolDepth.Shallow);
                }
                break;
            case BloodPoolDepth.Deep:
                if (cell.state == Cell.StateType.Occupied && cell.occupant.tag == "Player")
                {
                    BloodSoup.instance.OnBloodPool(BloodPoolDepth.Deep);
                }
                break;
        }
    }

    public void SetBloodPool(BloodPoolDepth condition)//设置血池
    {
        bloodPoolDepth = condition;
        switch (bloodPoolDepth)
        {
            case BloodPoolDepth.Shallow:
                Destroy(bloodPool);
                sustainTurn = 2;
                //从预制件中实例化血池
                bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Shallow"), transform);
                break;

            case BloodPoolDepth.Deep:
                Destroy(bloodPool);
                sustainTurn = 2;
                //从预制件中实例化血池
                bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Deep"), transform);
                break;

            case BloodPoolDepth.Normal:
                Destroy(bloodPool);
                ChessboardManager.instance.ChangeProperty(cell.cellLocation, null);
                break;
        }
    }
}
