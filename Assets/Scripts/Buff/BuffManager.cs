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
            //查询是否可叠加
            if (buff.canBeLayed)
            {
                buff.layer += 1;
                buff.OnAdd();
                //刷新持续时间
            }
            else
            {
                //刷新持续时间
            }
        }
        else
        {
            //实例化buff
            GameObject buffObj = Instantiate(Resources.Load("Prefabs/Buff/" + buff.name)) as GameObject;    
            //设置buff的持有者
            buffObj.GetComponent<BuffBase>().chessBase=buffTaker;
            //添加到buff列表
            buffTaker.buffList.Add(buffObj.GetComponent<BuffBase>());
            //触发buff的OnAdd
            buffObj.GetComponent<BuffBase>().OnAdd();
        }
       
    }
    //删除buff
    public void DeleteBuff(string buffName,ChessBase buffTaker)
    {
        Debug.Log("触发deleteBuff");
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
