using UnityEngine;
using DG.Tweening;
using System;

public class ChessBase : MonoBehaviour //棋子基类
{
    public ChessboardManager chessboardManager;
    public UnityEngine.UI.Image HPBar;//血条
    public UnityEngine.UI.Image DamageHPBar;//受伤血条
    public UnityEngine.UI.Image CureHPBar;//治疗血条
    public UnityEngine.UI.Image BarrierBar;//护盾条
    public Canvas HPBarCanvasPrefab;//血条画布
    public Canvas HPBarCanvasInstance;//血条画布实例

    // 共有属性
    public string chessName = "Chess";//棋子名称
    public int MaxHp = 10;//最大生命值
    public int Hp = 10;
    public int HP//当前生命值
    {
        get { return Hp; }
        set { Hp = Mathf.Clamp(value, 0, MaxHp); }
    }

    public int barrier = 0;//护盾值
    public int Barrier
    {
        get { return barrier; }
        set { 
                barrier = Mathf.Max(0, value);
                BarrierBar.DOFillAmount((float)Barrier / MaxHp, 0.5f);
            }
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

    public virtual void Start()
    {
        GameObject HPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/HealthBar");

        // 创建血条画布实例
        GameObject HPBarInstance = Instantiate(HPBarPrefab, transform.position, Quaternion.identity, transform);
        HPBarCanvasInstance = HPBarInstance.GetComponent<Canvas>();

        // 设置血条画布的位置
        RectTransform rt = HPBarCanvasInstance.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 1); // 这里的值可能需要根据你的游戏进行调整
        rt.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);

        // 获取血条
        HPBar = HPBarCanvasInstance.transform.Find("Bar").GetComponent<UnityEngine.UI.Image>();
        DamageHPBar = HPBarCanvasInstance.transform.Find("DamageBar").GetComponent<UnityEngine.UI.Image>();
        BarrierBar = HPBarCanvasInstance.transform.Find("BarrierBar").GetComponent<UnityEngine.UI.Image>();
        CureHPBar = HPBarCanvasInstance.transform.Find("CureBar").GetComponent<UnityEngine.UI.Image>();

        // 初始化血条的形状
        HPBar.fillAmount = (float)HP / MaxHp;
        CureHPBar.fillAmount = (float)HP / MaxHp;
        DamageHPBar.fillAmount = (float)HP / MaxHp;
        BarrierBar.fillAmount = (float)Barrier / MaxHp;
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

        //计算剩余移动数
        int residualDistance = Mathf.Abs(Mathf.Abs(direction.x) + Mathf.Abs(direction.y) - moveDistance);

        //更新Location
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
                        MeleeAttack(roadblockObject, residualDistance);
                    }
                    break;
                case "Player":
                    if(this.gameObject.tag == "Enemy")
                    {
                        //攻击玩家
                        MeleeAttack(roadblockObject, residualDistance);
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
    /// <param name="residualDistance">剩余移动数。(用于计算造成近战伤害的次数）</param>
    /// <remarks>在子类中需要添加攻击动画。</remarks>
    public virtual void MeleeAttack(GameObject roadblockObject, int residualDistance)
    {
        //近战攻击
            ChessBase AttackedChess = roadblockObject.GetComponent<ChessBase>();

            Vector2 originalPosition = transform.position;  //保存原始位置
            Vector2 targetPosition = originalPosition + (new Vector2(roadblockObject.transform.position.x, roadblockObject.transform.position.y) - originalPosition) * 0.75f;  //目标位置
            float moveDuration = 0.7f;  //移动所需的时间
            //创建一个序列
            DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();
            //添加前往目标位置的动画
            sequence.Append(transform.DOMove(targetPosition, moveDuration));
            //添加返回原始位置的动画
            sequence.Append(transform.DOMove(originalPosition, moveDuration));
            //在动画播放完毕后执行近战伤害判断
            sequence.OnComplete(() => {
                AttackedChess.TakeDamage(MeleeAttackPower * residualDistance);//攻击伤害 = 攻击力 * 剩余移动数
                int injury = AttackedChess.MeleeAttackPower;
                TakeDamage(injury);
            });
            //开始动画
            sequence.Play();
    }

    // 受伤方法
    public virtual void TakeDamage(int damage)
    {
        int damageTaken = damage - Barrier;
        Barrier -= damage;
        if (damageTaken > 0)
        {
            HP -= damageTaken;
            // 更新红色血条的形状
            HPBar.fillAmount = (float)HP / MaxHp;
            // 更新绿色血条的形状
            CureHPBar.fillAmount = (float)HP / MaxHp;
            // 延迟更新黄色血条的形状
            Invoke("UpdateDamageHPBar", 0.4f); // 延迟0.4秒
            Debug.Log(gameObject.name + "受到了" + damageTaken + "点伤害");
            
        }
    }

    // 黄色血条动画
    public void UpdateDamageHPBar()
    {
        DamageHPBar.DOFillAmount((float)HP / MaxHp, 0.39f) // 使用DoTween创建血条填充动画，动画持续0.39秒
        .OnComplete(() => // 在动画结束后执行以下代码
        {
            if (HP <= 0)
            {
                Death();
            }
        });
    }

    // 治疗方法
    public virtual void Cure(int cureValue)
    {
        HP += cureValue;
        // 更新绿色血条的形状
        CureHPBar.fillAmount = (float)HP / MaxHp;
        // 延迟更新蓝色血条的形状
        Invoke("UpdateHPBar", 0.5f); // 延迟0.5秒
        Debug.Log(gameObject.name + "受到了" + cureValue + "点治疗");
    }

    // 治疗时红色血条动画,治疗完成后更新血条
    public void UpdateHPBar()
    {
        HPBar.DOFillAmount((float)HP / MaxHp, 0.5f).OnComplete(() =>
        {
            DamageHPBar.fillAmount = (float)HP / MaxHp;
        });
    }

    // 死亡方法
    public virtual void Death()
    {
        //先播放死亡动画

        Debug.Log(gameObject.name + "死了");
        
        chessboardManager.RemoveChess(gameObject);
    }
}
