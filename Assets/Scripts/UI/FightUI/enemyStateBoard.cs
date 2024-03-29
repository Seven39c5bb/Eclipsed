using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class enemyStateBoard : MonoBehaviour, IPointerClickHandler
{
    //public string chessName;
    public TextMeshProUGUI chessHealth;
    public string chessBarrier;
    public string description;
    public UnityEngine.UI.Image cardImg;
    public EnemyBase thisEnemy;

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

    public GameObject EntryExplanation;

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
    }
    private void Update()
    {
        //��ʾ�غ�
        if(thisEnemy.isActed) { turnTip.GetComponent<CanvasGroup>().DOFade(1, 0.2f); }
        if (!thisEnemy.isActed && turnTip.GetComponent<CanvasGroup>().alpha == 1) { turnTip.GetComponent<CanvasGroup>().DOFade(0, 0.2f); }
        //Ѫ���仯
        chessHealth.text = thisEnemy.HP.ToString();
        if(healthImage.fillAmount != (float)thisEnemy.HP / thisEnemy.MaxHp) { healthImage.DOFillAmount((float)thisEnemy.HP / thisEnemy.MaxHp, 0.5f); }

        if(isClicked && thisEnemy != null)
        {
            chessName.text = thisEnemy.chessName;
            HealthText.text = "生命值: " + thisEnemy.HP;
            BarrierText.text = "护盾值: " + thisEnemy.Barrier;
            MobilityText.text = "行动力: " + thisEnemy.mobility;
            MoveModeText.text = "行为模式: " + thisEnemy.moveMode;
            MeleeAttackText.text = "近战伤害: " + thisEnemy.MeleeAttackPower;
            CardDescription.text = description;
        }
        
    }

    public bool isClicked = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(thisEnemy != null)
        {
            if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) 
            {
                detailedPanel.GetComponent<CanvasGroup>().alpha = 1;
                detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                isClicked = true;
            } 
            else if(isClicked == true)
            {
                detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
                detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                isClicked = false;
            }
            else//此面板未被点击，但具体信息面板被激活，说明其他面板被点击，将其他面板的点击状态设为false
            {
                enemyStateBoard[] enemyStateBoards = GameObject.FindObjectsOfType<enemyStateBoard>();
                foreach (enemyStateBoard board in enemyStateBoards)
                {
                    board.isClicked = false;
                }
                stateBoard.instance.isClicked = false;
                isClicked = true;
            }
            detailedPanel.GetComponent<CanvasGroup>().DOFade(detailedPanel.GetComponent<CanvasGroup>().alpha, 0.2f);
        }
    }
}
