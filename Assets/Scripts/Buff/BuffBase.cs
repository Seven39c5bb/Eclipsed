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
    public enum BuffImgType
    {
        //伤害
        Damage,
        //防御
        Defense,
        //生命
        HP,
        //行动力
        Action,
        //费用
        Cost
    }
    //buff图标类型
    public BuffImgType buffImgType;
    //buff名字
    //public string buffName;
    public string buffName => GetType().Name;
    public string buffNameCN;//用于面板显示的中文名字
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
    public int layer = 1;

    //造成伤害时触发
    public virtual int OnHit(int damage, ChessBase target, DamageType damageType = DamageType.Null)
    {
        //子类实现
        return damage;
    }
    //受到伤害时触发
    public virtual int OnHurt(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        //子类实现
        return damage;
    }
    //生命值减少后触发
    public virtual void OnHPReduce(int damage)
    {
        //子类实现
    }
    //回合开始时抽牌开始前触发
    public virtual void OnTurnStart()
    {
        //子类实现
    }
    //回合开始时抽牌结束后触发
    public virtual void OnTurnStartEndDraw()
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
    //不可叠加的buff重复添加的时候触发
    public virtual void OnUnlayerBuffRepeatAdd()
    {
        //子类实现
    }
    //buff移除的时候触发
    public virtual void OnRemove()
    {
        //子类实现
    }
    //抽卡的时候触发
    public virtual void OnDrawCard(Card card)
    {
        //子类实现
    }
    //使用卡牌的时候触发
    public virtual void OnUseCard(Card card)
    {
        //子类实现
    }

    //当弃牌的时候触发
    public virtual void OnDisCardCard(Card card)
    {
        //子类实现
    }

    //当玩家回合开始时触发
    public virtual void OnPlayerTurnBegin()
    {

    }

    //当玩家使用指向性卡牌指定怪物时
    public virtual bool OnPlayerUsePointerCardToEnemy() { return true; }

    //棋子移动时触发
    public virtual Vector2Int OnChessMove(Vector2Int direction)
    {
        //子类实现
        return direction;
    }
}
