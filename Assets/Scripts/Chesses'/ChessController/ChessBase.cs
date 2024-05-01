using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class ChessBase : MonoBehaviour //棋子基类
{
    // 血条组件
    public UnityEngine.UI.Image HPBar;//血条
    public UnityEngine.UI.Image DamageHPBar;//受伤血条
    public UnityEngine.UI.Image CureHPBar;//治疗血条
    public UnityEngine.UI.Image BarrierBar;//护盾条
    public Canvas HPBarCanvasPrefab;//血条画布
    public Canvas HPBarCanvasInstance;//血条画布实例

    // 共有属性
    public string chessName = "Chess";//棋子名称
    public List<BuffBase> buffList = new List<BuffBase>();//buff列表
    public bool DontMeleeAttack = false;//是否不主动碰撞
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
        //chessboardManager = GameObject.Find("Chessboard").GetComponent<ChessboardManager>();
    }

    GameObject HPBarInstance;
    public virtual void Start()
    {
        GameObject HPBarPrefab = Resources.Load<GameObject>("Prefabs/UI/HealthBar");

        // 创建血条画布实例
        HPBarInstance = Instantiate(HPBarPrefab, transform.position, Quaternion.identity, transform);
        HPBarCanvasInstance = HPBarInstance.GetComponent<Canvas>();

        // 获取精灵的边界
        Bounds spriteBounds = GetComponent<SpriteRenderer>().bounds;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 pivot = spriteRenderer.sprite.pivot;
        float pivotY = pivot.y / spriteRenderer.sprite.rect.height;

        // 获取精灵的宽度
        float spriteWidth = spriteBounds.size.x;

        // 计算血条应该放置的位置
        float barPositionY = spriteBounds.size.y;
        Debug.Log(name + "spriteBounds.min.y: " + spriteBounds.min.y + " spriteBounds.max.y: " + spriteBounds.max.y + " barPositionY: " + barPositionY + " pivotY " + pivotY); // "spriteBounds.min.y: -0.5 spriteBounds.max.y: 0.5 barPositionY: 0.5

        // 设置血条画布的位置
        RectTransform rt = HPBarCanvasInstance.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0, barPositionY + 0.25f - pivotY); // 这里的值可能需要根据你的游戏进行调整
        rt.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);

        // 设置血条的宽度
        rt.sizeDelta = new Vector2(spriteWidth * 1.1f, rt.sizeDelta.y);

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


    private int originOrientation = -1;//初始朝向(左)
    /// <summary>
    /// 移动方法
    /// </summary>
    /// <param name="parameters">执行任务所需的参数。</param>
    /// <remarks>
    /// 子类实现注意事项：
    /// 1. 需要添加移动动画。
    /// </remarks>
    public virtual (int residualDistance, bool isMeleeAttack) Move(Vector2Int direction)
    {

        //移动
        (Vector2 aimPosition, Vector2Int aimLocation, string roadblockType, GameObject roadblockObject) = 
        ChessboardManager.instance.MoveControl(gameObject, Location, direction);

        //计算Location和aimLocation之间的距离
        int moveDistance = Mathf.Abs(aimLocation.x - Location.x) + Mathf.Abs(aimLocation.y - Location.y);

        //计算剩余移动数
        int residualDistance = Mathf.Abs(Mathf.Abs(direction.x) + Mathf.Abs(direction.y) - moveDistance);

        //更新Location
        Location = aimLocation;

        bool isMeleeAttack = false;
        //判断是否发生近战攻击（不能放在回调函数中，因为回调函数是异步的）
        if (DontMeleeAttack == false && roadblockObject != null)
        {
            //根据该棋子的不同分类，对不同的障碍物做出不同的处理
            switch (roadblockType)
            {
                case "Enemy":
                    if(this.gameObject.tag == "Player")
                    {
                        //攻击敌人
                        isMeleeAttack = true;
                    }
                    break;
                case "Player":
                    if(this.gameObject.tag == "Enemy")
                    {
                        //攻击玩家
                        isMeleeAttack = true;
                    }
                    break;
                default:
                    break;
            }
        }


        if (direction.x == 0 || Math.Sign(direction.x) == originOrientation) //如果是向上或者向下移动，或者是向左移动
        {
            MoveToTargetPosition();
        }
        else
        {
            //更新朝向
            originOrientation = Math.Sign(direction.x);
            //翻转棋子
            if (direction.x < 0)
            {
                transform.DORotate(new Vector3(-45, 0, 0), 0.5f).OnComplete(MoveToTargetPosition);
                HPBarInstance.transform.localRotation = Quaternion.Euler(-45, 0, 0);
            }
            else if (direction.x > 0)
            {
                transform.DORotate(new Vector3(45, 180, 0), 0.5f).OnComplete(MoveToTargetPosition);
                HPBarInstance.transform.localRotation = Quaternion.Euler(45, 180, 0);
            }
        }

        void MoveToTargetPosition()
        {
            //将该棋子移动到目标位置aimPosition（尝试使用DOTween），需要判断何时移动完成
            float moveDuration = 0.5f * moveDistance;  //移动所需的时间
            transform.DOMove(aimPosition, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                //移动完成后执行的代码
                if (DontMeleeAttack == false && roadblockObject != null)
                {
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
                }
            });
        }

        return (residualDistance, isMeleeAttack);
    }

    /// <summary>
    /// 传送方法，传送到指定位置，在子类中需要添加传送特效。
    public virtual void Teleport(Vector2Int aimLocation)
    {
        //请求传送，这里棋子和棋盘的信息都会被更新
        bool success = ChessboardManager.instance.TeleportRequest(this, aimLocation);
        
        //传送特效写在子类中

        if (success)
        {
            //传送成功
            Debug.Log(gameObject.name + "传送到了" + aimLocation);
            gameObject.transform.position = ChessboardManager.instance.cellStates[Location.x, Location.y].transform.position;
        }
        else
        {
            //传送失败
            Debug.Log(gameObject.name + "传送失败");
        }
        
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
            float moveDuration = 0.5f;  //移动所需的时间

            // 计算攻击方向
            Vector3 attackDirection = (roadblockObject.transform.position - transform.position).normalized;
            //创建一个序列
            DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();
            //添加前往目标位置的动画
            sequence.Append(transform.DOMove(targetPosition, moveDuration).OnComplete(() => {
                // 添加被撞方抖动的动画，抖动的方向是攻击方向
                Vector3 shakePosition = roadblockObject.transform.position + attackDirection * 0.15f;
                DG.Tweening.Sequence sequence1 = DG.Tweening.DOTween.Sequence();
                sequence1.Append(roadblockObject.transform.DOMove(shakePosition, 0.15f).SetEase(Ease.OutQuad).SetDelay(0.1f));
                sequence1.Append(roadblockObject.transform.DOMove(roadblockObject.transform.position, 0.15f).SetEase(Ease.InQuad));
                sequence1.Play();
                //在被撞方处实例化粒子特效
                GameObject hitEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Particle/CrashParticle"), roadblockObject.transform.position, Quaternion.identity);
            }));
            //添加返回原始位置的动画
            sequence.Append(transform.DOMove(originalPosition, moveDuration));
            //在动画播放完毕后执行近战伤害判断
            sequence.OnComplete(() => {
                //计算撞击伤害
                int crashDamage = MeleeAttackPower * residualDistance;//撞击伤害 = 攻击力 * 剩余移动数
                foreach (BuffBase buff in buffList)//根据自身buff列表对伤害进行处理
                {
                    crashDamage = buff.OnCrash(crashDamage, AttackedChess);
                }
                foreach (BuffBase buff in AttackedChess.buffList)//根据被撞者buff列表对伤害进行处理
                {
                    crashDamage = buff.BeCrashed(crashDamage, this);
                }
                AttackedChess.TakeDamage(crashDamage, this);

                //计算受反击伤害
                int injury = AttackedChess.MeleeAttackPower;
                foreach (BuffBase buff in AttackedChess.buffList)//根据被撞者buff列表对伤害进行处理
                {
                    injury = buff.OnCrash(injury, this);
                }
                foreach (BuffBase buff in buffList)//根据自身buff列表对伤害进行处理
                {
                    injury = buff.BeCrashed(injury, AttackedChess);
                }
                TakeDamage(injury, AttackedChess);
            });
            //开始动画
            sequence.Play();
    }

    // 受伤方法
    public virtual void TakeDamage(int damage, ChessBase attacker)
    {
        foreach (BuffBase buff in buffList)//根据自身buff列表对伤害进行处理
        {
            damage = buff.OnHurt(damage, attacker);
        }
        foreach (BuffBase buff in attacker.buffList)//根据攻击者buff列表对伤害进行处理
        {
            damage = buff.OnHit(damage, this);
        }

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
        
        ChessboardManager.instance.RemoveChess(gameObject);
    }
}
