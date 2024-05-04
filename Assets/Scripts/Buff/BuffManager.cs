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
        BuffBase currBuff = null;
        bool isBuffExist = false;
        foreach(var buff in buffTaker.buffList)//遍历buff列表，检查buff是否存在
        {
            if (buff.buffName == buffName)
            {
                currBuff = buff;
                isBuffExist = true;
            }
        }
        if (isBuffExist)
        {
            //查询是否可叠加
            if (currBuff.canBeLayed)
            {
                currBuff.layer += 1;
                currBuff.OnAdd();
                //刷新持续时间
            }
            else
            {
                //刷新持续时间
                currBuff.OnUnlayerBuffRepeatAdd();
            }
        }
        else
        {
            //实例化buff
            GameObject buffObj = Instantiate(Resources.Load("Prefabs/Buff/" + buffName)) as GameObject;    
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
        List<BuffBase> buffsToRemove = new List<BuffBase>();
        foreach(var buff in buffTaker.buffList)
        {
            if (buff.buffName == buffName)
            {
                buff.OnRemove();
                buffsToRemove.Add(buff);
                Destroy(buff.gameObject);
            }
        }
        StartCoroutine(RemoveBuffsNextFrame(buffsToRemove, buffTaker));
    }
    IEnumerator RemoveBuffsNextFrame(List<BuffBase> buffsToRemove, ChessBase buffTaker)
    {
        yield return null; // 等待下一帧
        foreach(var buff in buffsToRemove)
        {
            buffTaker.buffList.Remove(buff);
        }
    }
}
