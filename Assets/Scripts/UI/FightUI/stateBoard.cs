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


    private void Awake()
    {
    
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //detailedPanel.SetActive(!detailedPanel.activeSelf);
        if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) detailedPanel.GetComponent<CanvasGroup>().alpha = 1; else detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
        detailedPanel.GetComponent<CanvasGroup>().DOFade(detailedPanel.GetComponent<CanvasGroup>().alpha, 0.2f);
    }
}
