using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

// 伤害类型
public enum DamageType
{
    Null, //无类型伤害
    Melee, //近战伤害
    Remote, //远程伤害
}

public interface IMobileUnit
{
    void Execute();
    void Undo();
    bool IsCompleted { get; }
}
public class MoveUnit : IMobileUnit//移动命令单元
{
    private ChessBase chess;
    private Vector2Int direction;//移动方向(单位向量)
    public bool IsCompleted { get; private set; }
    private int originOrientation;//初始朝向

    /// <summary>
    /// 移动单元构造函数
    /// </summary>
    /// <param name="chess">移动的棋子</param>
    /// <param name="direction">移动的方向</param>
    public MoveUnit(ChessBase chess, Vector2Int direction)
    {
        this.chess = chess;
        this.direction = direction;
        IsCompleted = false;
    }

    public void Execute()
    {
        // 执行移动操作
        chess.StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        // 执行移动操作
        originOrientation = chess.originOrientation;
        if (direction.x == 0 || Math.Sign(direction.x) == originOrientation) //如果是向上或者向下移动，或者是向面朝方向移动
        {
            MoveToTargetPosition();
        }
        else
        {
            //更新朝向
            originOrientation = Math.Sign(direction.x);
            chess.originOrientation = originOrientation;
            //翻转棋子
            if (direction.x < 0)
            {
                chess.transform.DORotate(new Vector3(-45, 0, 0), 0.5f).OnComplete(MoveToTargetPosition);
                chess.HPBarInstance.transform.localRotation = Quaternion.Euler(-45, 0, 0);
            }
            else if (direction.x > 0)
            {
                chess.transform.DORotate(new Vector3(45, 180, 0), 0.5f).OnComplete(MoveToTargetPosition);
                chess.HPBarInstance.transform.localRotation = Quaternion.Euler(45, 180, 0);
            }
        }

        void MoveToTargetPosition()
        {
            //将该棋子移动到目标位置aimPosition（尝试使用DOTween），需要判断何时移动完成
            float moveDuration = 0.5f;  //移动所需的时间
            Vector2Int aimLocation = chess.location + direction;
            Vector2 aimPosition = ChessboardManager.instance.cellStates[aimLocation.x, aimLocation.y].transform.position;
            //当棋子离开该棋格，触发棋格OnExit效果
            ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].OnChessExit(chess);
            ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].property?.OnChessExit(chess);

            chess.transform.DOMove(aimPosition, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                //更新Location
                ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].state = Cell.StateType.Empty;
                ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].occupant = null;
                ChessboardManager.instance.cellStates[aimLocation.x, aimLocation.y].state = Cell.StateType.Occupied;
                ChessboardManager.instance.cellStates[aimLocation.x, aimLocation.y].occupant = chess.gameObject;
                chess.location = aimLocation;

                //当玩家“进入”该棋格，触发该效果
                ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].property?.OnChessEnter(chess);
                ChessboardManager.instance.cellStates[chess.location.x, chess.location.y].OnChessEnter(chess);
                // 标记为命令完成
                IsCompleted = true;
            });
        }

        yield break;
    }

    public void Undo()
    {
        // 撤销移动操作
    }
}
public class MeleeAttackUnit : IMobileUnit//近战攻击命令单元
{
    private ChessBase attacker;
    private ChessBase target;
    private int residualDistance;
    public bool IsCompleted { get; private set; }

    public MeleeAttackUnit(ChessBase attacker, ChessBase target, int residualDistance)
    {
        this.attacker = attacker;
        this.target = target;
        this.residualDistance = residualDistance;
        IsCompleted = false;
        attacker.OnMeleeAttackComplete -= () => HandleMeleeAttackComplete();
        attacker.OnMeleeAttackComplete += () => HandleMeleeAttackComplete();
    }

    private void HandleMeleeAttackComplete()
    {
        // 事件处理逻辑
        IsCompleted = true;
    }

    public void Execute()
    {
        // 执行近战攻击
        attacker.StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        // 攻击逻辑
        attacker.MeleeAttack(target.gameObject, residualDistance);

        // 标记为完成
        IsCompleted = true;
        yield break;
    }

    public void Undo()
    {
        // 撤销攻击操作
    }
}


public abstract class ChessBase : MonoBehaviour //棋子基类
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
    public string chessDiscrption = "";//棋子描述
    public List<BuffBase> buffList = new List<BuffBase>();//buff列表
    public bool dontMeleeAttack = false;//是否不主动碰撞
    public bool isDead;//是否死亡
    public int maxHp = 10;//最大生命值
    public int HP_private = 10;
    public int HP//当前生命值
    {
        get { return HP_private; }
        set { HP_private = Mathf.Clamp(value, 0, maxHp); }
    }

    public int barrier_private = 0;//护盾值
    public int barrier
    {
        get { return barrier_private; }
        set { 
                barrier_private = Mathf.Max(0, value);
                BarrierBar.DOFillAmount((float)barrier / maxHp, 0.5f);
            }
    }

    public int meleeAttackPower_private = 2;//近战攻击力
    public int meleeAttackPower
    {
        get { return meleeAttackPower_private; }
        set { meleeAttackPower_private = Mathf.Max(0, value); }
    }


    private float moveSpeed_private = 5.0f;//可能不需要的属性
    public float moveSpeed
    {
        get { return moveSpeed_private; }
        set { moveSpeed_private = Mathf.Max(0, value); }
    }

    public Vector2Int location_private;//棋盘坐标
    public Vector2Int location
    {
        get { return location_private; }
        //坐标的值在0-9之间
        set { location_private = new Vector2Int(Mathf.Clamp(value.x, 0, 9), Mathf.Clamp(value.y, 0, 9)); }
    }

    // 方法
    void Awake()
    {
        //chessboardManager = GameObject.Find("Chessboard").GetComponent<ChessboardManager>();
    }

    public GameObject HPBarInstance;
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
        HPBar.fillAmount = (float)HP / maxHp;
        CureHPBar.fillAmount = (float)HP / maxHp;
        DamageHPBar.fillAmount = (float)HP / maxHp;
        BarrierBar.fillAmount = (float)barrier / maxHp;
    }

    /* private int originOrientation = -1;//初始朝向(左)
    private List<Vector2Int> mobileUnits = new List<Vector2Int>(); //待执行的移动单元
    /// <summary>
    /// 移动方法
    /// </summary>
    /// <param name="parameters">执行任务所需的参数。</param>
    /// <remarks>
    /// 子类实现注意事项：
    /// 1. 需要添加移动动画。
    /// </remarks>
    public virtual (int residualDistance, bool isMeleeAttack, bool isRotate) Move(Vector2Int direction)
    {

        //移动
        (Vector2 aimPosition, Vector2Int aimLocation, string roadblockType, GameObject roadblockObject, List<Vector2Int> pastCellList) = 
        ChessboardManager.instance.MoveControl(gameObject, location, direction);

        //计算Location和aimLocation之间的距离
        int moveDistance = Mathf.Abs(aimLocation.x - location.x) + Mathf.Abs(aimLocation.y - location.y);

        //计算剩余移动数
        int residualDistance = Mathf.Abs(Mathf.Abs(direction.x) + Mathf.Abs(direction.y) - moveDistance);

        //当玩家离开该棋格
        if (location != aimLocation)
        {
            ChessboardManager.instance.cellStates[location.x, location.y].property?.OnChessExit(this);
        }
 
        //更新Location
        location = aimLocation;

        bool isMeleeAttack = false;
        //判断是否发生近战攻击（不能放在回调函数中，因为回调函数是异步的）
        if (dontMeleeAttack == false && roadblockObject != null)
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

        bool isRotate = false;
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
                isRotate = true;
                transform.DORotate(new Vector3(-45, 0, 0), 0.5f).OnComplete(MoveToTargetPosition);
                HPBarInstance.transform.localRotation = Quaternion.Euler(-45, 0, 0);
            }
            else if (direction.x > 0)
            {
                isRotate = true;
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
                //当玩家移动到该棋格，触发该效果
                ChessboardManager.instance.cellStates[location.x, location.y].property?.OnChessEnter(this);
                //移动完成后执行的代码
                if (dontMeleeAttack == false && roadblockObject != null)
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

        return (residualDistance, isMeleeAttack, isRotate);
    } */


    public int originOrientation = -1; //初始朝向(左)
    private List<IMobileUnit> mobileUnits = new List<IMobileUnit>(); //待执行的移动单元
    public virtual IEnumerator Move(Vector2Int direction)
    {
        // 获取命令列表
        List<IMobileUnit> commands = ChessboardManager.instance.MoveControl(gameObject, location, direction);

        while (commands.Count > 0)
        {
            // 执行第一个命令
            var command = commands[0];
            command.Execute();

            // 等待命令执行完成
            yield return new WaitUntil(() => command.IsCompleted);

            // 移除已执行的命令
            commands.RemoveAt(0);
        }
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
            //添加传送特效
            Material tpMat=Resources.Load<Material>("Shader/shaderTest/dissolve");
            this.GetComponent<SpriteRenderer>().material = tpMat;
            this.GetComponent<SpriteRenderer>().material.DOFloat(0, "_dissolveValue", 1f).OnComplete(() =>
            {
               gameObject.transform.position = ChessboardManager.instance.cellStates[location.x, location.y].transform.position;
               this.GetComponent<SpriteRenderer>().material.DOFloat(1, "_dissolveValue", 1f);
            });
            
        }
        else
        {
            //传送失败
            Debug.Log(gameObject.name + "传送失败");
        }
        
    }


    public event Action OnMeleeAttackComplete;//近战攻击完成事件
    /// <summary>
    /// 近战攻击方法
    /// </summary>
    /// <param name="roadblockObject">攻击对象对象。</param>
    /// <param name="residualDistance">剩余移动数。(用于计算造成近战伤害的次数）</param>
    /// <remarks>在子类中需要添加攻击动画。</remarks>
    public virtual void MeleeAttack(GameObject roadblockObject, int residualDistance)
    {
        // 近战攻击
        ChessBase AttackedChess = roadblockObject.GetComponent<ChessBase>();

        Vector2 originalPosition = transform.position;  // 保存原始位置
        Vector2 targetPosition = originalPosition + (new Vector2(roadblockObject.transform.position.x, roadblockObject.transform.position.y) - originalPosition) * 0.75f;  // 目标位置
        float moveDuration = 0.5f;  // 移动所需的时间

        // 计算攻击方向
        Vector3 attackDirection = (roadblockObject.transform.position - transform.position).normalized;
        // 创建一个序列
        DG.Tweening.Sequence sequence = DG.Tweening.DOTween.Sequence();
        // 添加前往目标位置的动画
        sequence.Append(transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InCubic).OnComplete(() => {
            // 添加被撞方抖动的动画，抖动的方向是攻击方向
            Vector3 shakePosition = roadblockObject.transform.position + attackDirection * 0.15f;
            DG.Tweening.Sequence sequence1 = DG.Tweening.DOTween.Sequence();
            sequence1.Append(roadblockObject.transform.DOMove(shakePosition, 0.15f).SetEase(Ease.OutCubic).SetDelay(0.1f));
            sequence1.Append(roadblockObject.transform.DOMove(roadblockObject.transform.position, 0.15f).SetEase(Ease.InCubic));
            sequence1.Play();
            // 在被撞方处实例化粒子特效
            GameObject hitEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Particle/CrashParticle"), roadblockObject.transform.position, Quaternion.identity);
        }));
        // 添加返回原始位置的动画
        sequence.Append(transform.DOMove(originalPosition, moveDuration).SetEase(Ease.OutCubic));
        // 在动画播放完毕后执行近战伤害判断
        sequence.OnComplete(() => {
            // 计算撞击伤害
            int crashDamage = meleeAttackPower * residualDistance; // 撞击伤害 = 攻击力 * 剩余移动数
            foreach (BuffBase buff in buffList) // 根据自身buff列表对伤害进行处理
            {
                crashDamage = buff.OnCrash(crashDamage, AttackedChess);
            }
            foreach (BuffBase buff in AttackedChess.buffList) // 根据被撞者buff列表对伤害进行处理
            {
                crashDamage = buff.BeCrashed(crashDamage, this);
            }
            AttackedChess.TakeDamage(crashDamage, this);

            // 计算受反击伤害
            int injury = AttackedChess.meleeAttackPower;
            foreach (BuffBase buff in AttackedChess.buffList) // 根据被撞者buff列表对伤害进行处理
            {
                injury = buff.OnCrash(injury, this);
            }
            foreach (BuffBase buff in buffList) // 根据自身buff列表对伤害进行处理
            {
                injury = buff.BeCrashed(injury, AttackedChess);
            }
            TakeDamage(injury, AttackedChess);

            // 触发事件
            OnMeleeAttackComplete?.Invoke();
        });
        // 开始动画
        sequence.Play();
    }

    /// <summary>
    /// 弹幕攻击方法
    /// </summary>
    /// <param name="bulletDamage">弹幕伤害</param>
    /// <param name="aimChess">目标棋子</param>
    /// <param name="bulletPrefab">子弹预制件</param>
    /// <param name="HitEffectPrefab"></param>
    /// <returns></returns>
    /// <remarks>子弹射出前有固定滞留时间delayDuration = 0.5f</remarks>
    public virtual void BulletAttack(int bulletDamage, ChessBase aimChess, GameObject bulletPrefab, GameObject HitEffectPrefab)
    {
        float delayDuration = 0.5f;

        // 弹幕攻击

        // 获取粒子系统组件
        ParticleSystem particleSystem = bulletPrefab.GetComponent<ParticleSystem>();
        // 计算目标与当前位置的距离
        float distance = Vector3.Distance(transform.position, aimChess.transform.position);
        // 根据距离计算移动时间
        float moveDuration = distance / 10; // 假设子弹的速度为3单位/秒
        /* // 设置粒子的生命周期
        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startLifetime = delayDuration + moveDuration;
        } */
        // 实例化子弹
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -1), Quaternion.identity);

        // 等待滞空时间再发射子弹
        /* Debug.Log("Before WaitForSeconds, delayDuration: " + delayDuration);
        yield return new WaitForSeconds(delayDuration);
        Debug.Log("After WaitForSeconds"); */

        Debug.Log(aimChess.transform.position + " " + aimChess.name);

        // 子弹移动到目标位置
        bullet.transform.DOMove(aimChess.transform.position, moveDuration).SetDelay(delayDuration).SetEase(Ease.InCubic).OnComplete(() =>
        {
            // 在目标位置实例化击中特效
            Instantiate(HitEffectPrefab, aimChess.transform.position, Quaternion.identity);
            // 对目标造成伤害
            aimChess.TakeDamage(bulletDamage, this);
            // 销毁子弹
            Destroy(bullet);
        });

    }

    // 受伤方法
    public virtual void TakeDamage(int damage, ChessBase attacker, DamageType damageType = DamageType.Null)
    {
        
        if (attacker != null)
        {
            foreach (BuffBase buff in buffList)//根据自身buff列表对伤害进行处理
            {
                damage = buff.OnHurt(damage, attacker, damageType);
            }
            foreach (BuffBase buff in attacker.buffList)//根据攻击者buff列表对伤害进行处理
            {
                damage = buff.OnHit(damage, this, damageType);
            }
        }

        int damageTaken = damage - barrier;
        barrier -= damage;
        if (damageTaken > 0)
        {
            HP -= damageTaken;
            if (HP <= 0)
            {
                isDead = true;
                foreach(BuffBase buff in buffList)
                {
                    buff.OnDie();
                }
            }
            // 更新红色血条的形状
            HPBar.fillAmount = (float)HP / maxHp;
            // 更新绿色血条的形状
            CureHPBar.fillAmount = (float)HP / maxHp;
            // 延迟更新黄色血条的形状
            Invoke("UpdateDamageHPBar", 0.4f); // 延迟0.4秒
            Debug.Log(gameObject.name + "受到了" + damageTaken + "点伤害");
            
        }
    }

    // 黄色血条动画
    public void UpdateDamageHPBar()
    {
        DamageHPBar.DOFillAmount((float)HP / maxHp, 0.39f) // 使用DoTween创建血条填充动画，动画持续0.39秒
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
        CureHPBar.fillAmount = (float)HP / maxHp;
        // 延迟更新蓝色血条的形状
        Invoke("UpdateHPBar", 0.5f); // 延迟0.5秒
        Debug.Log(gameObject.name + "受到了" + cureValue + "点治疗");
    }

    // 治疗时红色血条动画,治疗完成后更新血条
    public void UpdateHPBar()
    {
        HPBar.DOFillAmount((float)HP / maxHp, 0.5f).OnComplete(() =>
        {
            DamageHPBar.fillAmount = (float)HP / maxHp;
        });
    }

    // 自身回合开始时护盾衰减方法
    public void BarrierDecay()
    {
        // 护盾值减半
        barrier -= barrier / 2;
        // 使用DoTween使护盾闪烁
        BarrierBar.DOColor(new Color(38f/255f, 214/255f, 1, 0), 0.25f).OnComplete(() =>
        {
            BarrierBar.DOColor(new Color(38f/255f, 214/255f, 1, 139f/255f), 0.25f);
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
