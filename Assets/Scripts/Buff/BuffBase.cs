using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : MonoBehaviour
{
    //buff类型
    public enum BuffType
    {
        //增益
        Buff,
        //减益
        Debuff
    }
    //buff名字
    public string buffName;
    //buff持续回合数
    public int durationTurn;
    //确定挂在哪个棋子上
    public ChessBase chessBase;
    //buff类型
    public BuffType buffType;
    //buff描述
    public string description;

    public virtual void InovkeBuff()
    {
        //子类实现
    }
}
