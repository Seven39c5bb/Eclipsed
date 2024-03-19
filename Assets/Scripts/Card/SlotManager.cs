using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    //slots�б�
    public static List<Slot> slotsList;
    Transform slotAssem;
    private void Awake()
    {
        //��ȡslotAssem����
        slotAssem = GameObject.Find("slotAssem").GetComponent<Transform>();
        //��ʼ��slots�б�
        slotsList = new List<Slot>();
        //��10��slot�ӽ�slot�б�,�����г�ʼ��
        for(int i = 0; i < slotAssem.childCount; i++)
        {
            slotsList.Add(slotAssem.GetChild(i).GetComponent<Slot>());
            slotsList[i].instance.isEmpty = true;
        }
    }
}
