using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class stateBoard:MonoBehaviour,IPointerClickHandler
{
    //public string chessName;
    public TextMeshProUGUI chessHealth;
    public string chessBarrier;
    public string description;
    public UnityEngine.UI.Image cardImg;

    public GameObject detailedPanel; 
    public TextMeshProUGUI chessName;
    public TextMeshProUGUI detailedState;
    public TextMeshProUGUI CardDescription;
    public UnityEngine.UI.Image healthImage;
    public GameObject turnTip;

    private TextMeshProUGUI HealthText;
    private TextMeshProUGUI BarrierText;
    private TextMeshProUGUI MobilityText;
    private TextMeshProUGUI MoveModeText;
    private TextMeshProUGUI MeleeAttackText;
    
    public static stateBoard instance;
    public List<GameObject> buffBlockList = new List<GameObject>();

    public GameObject buffBlockPrefab;
    public int preBuffListCount = 0;

    private void Awake()
    {
        instance = this;

        detailedPanel = GameObject.Find("detailedStatePanel");
        chessName=GameObject.Find("chessName").GetComponent<TextMeshProUGUI>();
        CardDescription = GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>();
        detailedState = GameObject.Find("detailedState").GetComponent<TextMeshProUGUI>();

        //从detailedPanel中获取组件
        HealthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        BarrierText = GameObject.Find("BarrierText").GetComponent<TextMeshProUGUI>();
        MobilityText = GameObject.Find("MobilityText").GetComponent<TextMeshProUGUI>();
        MoveModeText = GameObject.Find("MoveModeText").GetComponent<TextMeshProUGUI>();
        MeleeAttackText = GameObject.Find("MeleeAttackText").GetComponent<TextMeshProUGUI>();

        tooltipPanel = GameObject.Find("EntryExplanation");
        EntryNameText = GameObject.Find("EntryNameText").GetComponent<TextMeshProUGUI>();
        EntryExplanationText = GameObject.Find("EntryExplanationText").GetComponent<TextMeshProUGUI>();

        //加载buffBlock预制体
        buffBlockPrefab = Resources.Load<GameObject>("Prefabs/BuffBlock");
    }
    private void Update()
    {
        //收到伤害时血条变透明
        //float healthAlpha = PlayerController.instance.HP / PlayerController.instance.MaxHp;
        //healthCanvasGroup.alpha = healthAlpha;
        chessHealth.text=PlayerController.instance.Hp.ToString();//test
        healthImage.DOFillAmount((float)PlayerController.instance.Hp / PlayerController.instance.MaxHp, 0.5f);
        if (FightManager.instance.curFightType == FightType.Player)
        {
            turnTip.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        }
        else if(FightManager.instance.curFightType != FightType.Player && turnTip.GetComponent<CanvasGroup>().alpha == 1)
        { turnTip.GetComponent<CanvasGroup>().DOFade(0, 0.2f); }
        //test
        //foreach(var enemy in ChessboardManager.instance.enemyControllerList)
        //{
        //    if(enemy.isActed) { //显示当前board高亮
        //        turnTip.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        //    }
        //    if(!enemy.isActed && turnTip.GetComponent<CanvasGroup>().alpha==1) { turnTip.GetComponent<CanvasGroup>().DOFade(0, 0.2f); }
        //}

        if (isClicked)
        {
            chessName.text = PlayerController.instance.chessName;
            CardDescription.text = description;
            HealthText.text = "生命值: " + PlayerController.instance.Hp;
            BarrierText.text = "护盾值: " + PlayerController.instance.Barrier;
            MobilityText.text = "行动力: --";
            MoveModeText.text = "行为模式: --";
            MeleeAttackText.text = "近战伤害: " + PlayerController.instance.MeleeAttackPower;

            if (preBuffListCount != PlayerController.instance.buffList.Count)
            {
                foreach (var buff in buffBlockList)
                {
                    Destroy(buff);
                }
                buffBlockList.Clear();
                foreach (var buff in PlayerController.instance.buffList)
                {
                    GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                    buffBlockList.Add(buffBlock);
                    buffBlock.GetComponent<buffBoard>().buff = buff;
                }
                preBuffListCount = PlayerController.instance.buffList.Count;
            }
        }

        if (isTooltipActive)
        {
            tooltipPanel.transform.position = Input.mousePosition;
        }
    }

    public bool isClicked = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        //detailedPanel.SetActive(!detailedPanel.activeSelf);
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) 
        {
            foreach (var buff in buffBlockList)
            {
                Destroy(buff);
            }
            buffBlockList.Clear();
            detailedPanel.GetComponent<CanvasGroup>().alpha = 1;
            detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            //从playerController中获取buff列表，逐一生成预制体BuffBlock并赋值
            foreach (var buff in PlayerController.instance.buffList)
            {
                GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                buffBlockList.Add(buffBlock);
                buffBlock.GetComponent<buffBoard>().buff = buff;
            }
            preBuffListCount = PlayerController.instance.buffList.Count;

            isClicked = true;
        } 
        else if(isClicked == true)
        {
            detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
            detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            isClicked = false;
        }
        else//此面板未被点击，但具体信息面板已被打开，说明其他面板被点击，将其关闭
        {
            //查询所有的enemyStateBoard，将其isClicked设为false
            enemyStateBoard[] enemyStateBoards = GameObject.FindObjectsOfType<enemyStateBoard>();
            foreach (enemyStateBoard enemyStateBoard in enemyStateBoards)
            {
                enemyStateBoard.isClicked = false;
            }
            foreach (var buff in buffBlockList)
            {
                Destroy(buff);
            }
            buffBlockList.Clear();
            foreach (var buff in PlayerController.instance.buffList)
            {
                GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                buffBlockList.Add(buffBlock);
                buffBlock.GetComponent<buffBoard>().buff = buff;
            }
            preBuffListCount = PlayerController.instance.buffList.Count;

            isClicked = true;
        }
        detailedPanel.GetComponent<CanvasGroup>().DOFade(detailedPanel.GetComponent<CanvasGroup>().alpha, 0.2f);
        //Debug.Log(thisEnemy.name+" + "+ thisEnemy.HP);
    }

    


    //用于词条解释的方法
    public GameObject tooltipPanel; // 面板
    public TextMeshProUGUI EntryNameText; // 文本
    public TextMeshProUGUI EntryExplanationText; // 文本
    private bool isTooltipActive = false;
    public void ShowTooltip(string tooltipString, string tooltipName)
    {
        // 设置面板的位置和文本
        isTooltipActive = true;
        EntryNameText.text = tooltipName;
        EntryExplanationText.text = tooltipString;
        tooltipPanel.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void HideTooltip()
    {
        tooltipPanel.GetComponent<CanvasGroup>().alpha = 0;
        isTooltipActive = false;
    }

    public void OnHealthEnter()
    {
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("角色所拥有的生命力数值。当生命值跌落至0或以下时，该角色将会死亡。", "生命值");}
    }

    public void OnBarrierEnter()
    {
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("用于抵挡即将受到的伤害，会优先于生命值扣除。", "护盾值");}
    }

    public void OnMobilityEnter()
    {
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("决定该角色在自己回合能进行移动的次数。如：行动力为2的角色，则它的回合内可以进行2次移动。", "行动力");}
    }

    public void OnMoveModeEnter()
    {
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("决定该角色每次移动行动的格数，有：行走（每次走一格）、跳跃（每次走三格）、工具（每次走两格）、飞行（每次走四格）、特殊（根据怪物特性决定）等。", "行动模式");}
    }

    public void OnMeleeAttackEnter()
    {
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1) 
        ShowTooltip("角色的近战伤害数值决定角色对敌人进行碰撞时造成的伤害。角色进行行动与敌人发生碰撞时，会停在行动方向上、被碰撞目标的身前一格，并对被碰撞目标造成 “自身近战伤害 × 发生碰撞时本该移动的格数” 的伤害，且自身受到一次敌方的近战伤害。", "近战伤害");
    }


}
