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

        if(isClicked)
        {
            chessName.text = thisEnemy.name;
            HealthText.text = "currHealth: " + thisEnemy.HP;
            BarrierText.text = "Barrier: " + thisEnemy.Barrier;
            MobilityText.text = "Mobility: " + thisEnemy.mobility;
            MoveModeText.text = "MoveMode: " + thisEnemy.moveMode;
            MeleeAttackText.text = "MeleeAttack: " + thisEnemy.MeleeAttackPower;
            CardDescription.text = description;
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
}
