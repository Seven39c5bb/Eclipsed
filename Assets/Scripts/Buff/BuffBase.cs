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
    //是否可叠加
    public bool canBeLayed;
    //buff层数
    public int layer;

    //造成伤害时触发
    public virtual int OnHit(int damage, ChessBase target)
    {
        //子类实现
        return damage;
    }
    //受到伤害时触发
    public virtual int OnHurt(int damage, ChessBase attacker)
    {
        //子类实现
        return damage;
    }
    //回合开始时触发
    public virtual void OnTurnStart()
    {
        //子类实现
    }
    //回合结束时触发
    public virtual void OnTurnEnd()
    {
        //子类实现
    }
    //碰撞时触发
    public virtual int OnCrash(int damage, ChessBase target)
    {
        //子类实现
        return damage;
    }
    //被碰撞时触发
    public virtual int BeCrashed(int damage, ChessBase attacker)
    {
        //子类实现
        return damage;
    }
    //死亡时触发
    public virtual void OnDie()
    {
        //子类实现
    }
    //buff添加的时候触发
    public virtual void OnAdd()
    {
        //子类实现
    }
    //buff移除的时候触发
    public virtual void OnRemove()
    {
        //子类实现
    }
}
