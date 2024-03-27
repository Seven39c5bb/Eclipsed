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
    



    private void Awake()
    {
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
            chessName.text = PlayerController.instance.name;
            CardDescription.text = description;
            HealthText.text = "currHealth: " + PlayerController.instance.Hp;
            BarrierText.text = "Barrier: " + PlayerController.instance.Barrier;
            MobilityText.text = "Mobility: --";
            MoveModeText.text = "MoveMode: --";
            MeleeAttackText.text = "MeleeAttack: " + PlayerController.instance.MeleeAttackPower;
        }

        if (isTooltipActive)
        {
            tooltipPanel.transform.position = Input.mousePosition;
        }
    }

    bool isClicked = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        //detailedPanel.SetActive(!detailedPanel.activeSelf);
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) 
        {
            detailedPanel.GetComponent<CanvasGroup>().alpha = 1;
            isClicked = true;
        } 
        else 
        {
            detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
            isClicked = false;
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
        Debug.Log("Health");
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("This is the detailed explanation of health", "Health");}
    }

    public void OnBarrierEnter()
    {
        Debug.Log("Barrier");
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("This is the detailed explanation of barrier", "Barrier");}
    }

    public void OnMobilityEnter()
    {
        Debug.Log("Mobility");
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("This is the detailed explanation of mobility", "Mobility");}
    }

    public void OnMoveModeEnter()
    {
        Debug.Log("Move Mode");
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1)
        {ShowTooltip("This is the detailed explanation of move mode", "Move Mode");}
    }

    public void OnMeleeAttackEnter()
    {
        Debug.Log("Melee Attack");
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 1) ShowTooltip("This is the detailed explanation of melee attack", "Melee Attack");
    }


}
