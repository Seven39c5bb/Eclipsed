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
    public CanvasGroup healthCanvasGroup;
    public GameObject turnTip;


    private void Awake()
    {
        detailedPanel = GameObject.Find("detailedStatePanel");
        chessName=GameObject.Find("chessName").GetComponent<TextMeshProUGUI>();
        CardDescription = GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>();
        detailedState = GameObject.Find("detailedState").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        //提示回合
        if(thisEnemy.isActed) { turnTip.GetComponent<CanvasGroup>().DOFade(1, 0.2f); }
        if (!thisEnemy.isActed && turnTip.GetComponent<CanvasGroup>().alpha == 1) { turnTip.GetComponent<CanvasGroup>().DOFade(0, 0.2f); }
        //血量变化
        chessHealth.text = thisEnemy.HP.ToString();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //detailedPanel.SetActive(!detailedPanel.activeSelf);
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) detailedPanel.GetComponent<CanvasGroup>().alpha = 1; else detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
        detailedPanel.GetComponent<CanvasGroup>().DOFade(detailedPanel.GetComponent<CanvasGroup>().alpha, 0.2f);
        //Debug.Log(thisEnemy.name+" + "+ thisEnemy.HP);
    }
}
