using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChessBase : MonoBehaviour //棋子基类
{
    public ChessboardManager chessboardManager;

    // 共有属性
    public int MaxHp = 10;//最大生命值
    public int hp = 10;
    public int HP//当前生命值
    {
        get { return hp; }
        set { hp = Mathf.Clamp(value, 0, MaxHp); }
    }

    public int barrier = 0;//护盾值
    public int Barrier
    {
        get { return barrier; }
        set { barrier = Mathf.Max(0, value); }
    }

    public int meleeAttackPower = 2;//近战攻击力
    public int MeleeAttackPower
    {
        get { return meleeAttackPower; }
        set { meleeAttackPower = Mathf.Max(0, value); }
    }


    private float moveSpeed = 5.0f;//可能不需要的属性
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = Mathf.Max(0, value); }
    }

    public Vector2Int location;//棋盘坐标
    public Vector2Int Location
    {
        get { return location; }
        //坐标的值在0-9之间
        set { location = new Vector2Int(Mathf.Clamp(value.x, 0, 9), Mathf.Clamp(value.y, 0, 9)); }
    }

    // 方法
    void Awake()
    {
        chessboardManager = GameObject.Find("Chessboard").GetComponent<ChessboardManager>();
    }


    /// <summary>
    /// 移动方法
    /// </summary>
    /// <param name="parameters">执行任务所需的参数。</param>
    /// <remarks>
    /// 子类实现注意事项：
    /// 1. 需要添加移动动画。
    /// </remarks>
    public virtual void Move(Vector2Int direction)
    {

        //移动
        (Vector2 aimPosition, Vector2Int aimLocation, string roadblockType, GameObject roadblockObject) = 
        chessboardManager.MoveControl(gameObject, Location, direction);

        //计算Location和aimLocation之间的距离
        int moveDistance = Mathf.Abs(aimLocation.x - Location.x) + Mathf.Abs(aimLocation.y - Location.y);

        Location = aimLocation;

        //将该棋子移动到目标位置aimPosition（尝试使用DOTween），需要判断何时移动完成
        float moveDuration = 0.5f * moveDistance;  //移动所需的时间
        transform.DOMove(aimPosition, moveDuration).OnComplete(() =>
        {
            //移动完成后执行的代码
            //根据该棋子的不同分类，对不同的障碍物做出不同的处理
            switch (roadblockType)
            {
                case "Enemy":
                    if(this.gameObject.tag == "Player")
                    {
                        //攻击敌人
                        MeleeAttack(roadblockObject);
                    }
                    break;
                case "Player":
                    if(this.gameObject.tag == "Enemy")
                    {
                        //攻击玩家
                        MeleeAttack(roadblockObject);
                    }
                    break;
                default:
                    break;
            }
        
        });

    }


    /// <summary>
    /// 近战攻击方法
    /// </summary>
    /// <param name="roadblockObject">攻击对象对象。</param>
    /// <remarks>在子类中需要添加攻击动画。</remarks>
    public virtual void MeleeAttack(GameObject roadblockObject)
    {
        //近战攻击
            ChessBase AttackedChess = roadblockObject.GetComponent<ChessBase>();

            Vector2 originalPosition = transform.position;  //保存原始位置
            Vector2 targetPosition = originalPosition + (new Vector2(roadblockObject.transform.position.x, roadblockObject.transform.position.y) - originalPosition) * 0.75f;  //目标位置
            float moveDuration = 0.7f;  //移动所需的时间
            //创建一个序列
            Sequence sequence = DOTween.Sequence();
            //添加前往目标位置的动画
            sequence.Append(transform.DOMove(targetPosition, moveDuration));
            //添加返回原始位置的动画
            sequence.Append(transform.DOMove(originalPosition, moveDuration));
            //开始动画
            sequence.Play();

            AttackedChess.TakeDamage(MeleeAttackPower);
            int injury = AttackedChess.MeleeAttackPower;
            TakeDamage(injury);
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
