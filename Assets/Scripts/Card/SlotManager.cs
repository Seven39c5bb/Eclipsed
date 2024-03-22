using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    //slots列表
    public static List<Slot> slotsList;
    Transform slotAssem;
    private void Awake()
    {
        //获取slotAssem物体
        slotAssem = GameObject.Find("slotAssem").GetComponent<Transform>();
        //初始化slots列表
        slotsList = new List<Slot>();
        //将10个slot加进slot列表,并进行初始化
        for(int i = 0; i < slotAssem.childCount; i++)
        {
            slotsList.Add(slotAssem.GetChild(i).GetComponent<Slot>());
            slotsList[i].instance.isEmpty = true;
        }
    }
}
