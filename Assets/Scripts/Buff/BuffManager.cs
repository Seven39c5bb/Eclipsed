using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager buff_instance;
    public static BuffManager instance
    {
        get
        {
            if (buff_instance == null)
            {
                buff_instance = GameObject.FindObjectOfType<BuffManager>();
            }
            return buff_instance;
        }
    }
    public void AddBuff(string buffName,ChessBase buffTaker)
    {
        BuffBase buff= Resources.Load("Prefabs/Buff/" + buffName).GetComponent<BuffBase>();
        if (buffTaker.buffList.Contains(buff))
        {
            //��ѯ�Ƿ�ɵ���
            if (buff.canBeLayed)
            {
                buff.layer += 1;
                buff.OnAdd();
                //ˢ�³���ʱ��
            }
            else
            {
                //ˢ�³���ʱ��
            }
        }
        else
        {
            //ʵ����buff
            GameObject buffObj = Instantiate(Resources.Load("Prefabs/Buff/" + buff.name)) as GameObject;    
            //����buff�ĳ�����
            buffObj.GetComponent<BuffBase>().chessBase=buffTaker;
            //��ӵ�buff�б�
            buffTaker.buffList.Add(buffObj.GetComponent<BuffBase>());
            //����buff��OnAdd
            buffObj.GetComponent<BuffBase>().OnAdd();
        }
       
    }
    //ɾ��buff
    public void DeleteBuff(string buffName,ChessBase buffTaker)
    {
        Debug.Log("����deleteBuff");
        foreach(var buff in buffTaker.buffList)
        {
            if (buff.name == buffName)
            {
                buffTaker.buffList.Remove(buff);
                Destroy(buff.gameObject);
            }
        }
    }
}
