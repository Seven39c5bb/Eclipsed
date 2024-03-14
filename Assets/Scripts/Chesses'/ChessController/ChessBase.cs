using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBase : MonoBehaviour //棋子基类
{
    ChessboardManager chessboardManager;

    // 共有属性
    public int MaxHp = 10;
    private int hp = 10;
    public int HP
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, MaxHp); }
    }

    private int barrier = 0;
    public int Barrier
    {
        get { return barrier; }
        set { barrier = Mathf.Max(0, value); }
    }

    private int meleeAttackPower = 2;
    public int MeleeAttackPower
    {
        get { return meleeAttackPower; }
        set { meleeAttackPower = Mathf.Max(0, value); }
    }


    private float moveSpeed = 5.0f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Max(0, value); }
    }



    // 方法
    void Awake()
    {
        chessboardManager = GameObject.Find("Chessboard").GetComponent<ChessboardManager>();
    }


    // 移动方法
    public virtual void Move()
    {
        //移动
    }

    //近战攻击方法
    public virtual void MeleeAttack()
    {
        //近战攻击
    }

    // 受伤方法
    public virtual void TakeDamage(int damage)
    {
        int damageTaken = damage - Barrier;
        Barrier -= damage;
        if (damageTaken > 0)
        {
            HP -= damageTaken;
            if (HP <= 0)
            {
                Death();
            }
        }
    }

    // 死亡方法
    public virtual void Death()
    {
        chessboardManager.RemoveChess(gameObject);
        Destroy(gameObject);
    }
}
