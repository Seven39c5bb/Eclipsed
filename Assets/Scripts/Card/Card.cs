﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class Card : UIBase,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    //枚举卡牌的种类
    public enum cardType
    {
        action,skill,rites,trap
    }
    //卡牌的种类
    public cardType type;
    //稀有度
    public enum rareType
    {
        common,rare,legend
    }
    public rareType rare;
    //卡牌名称
    public string cardName;
    //卡牌描述
    public string discription;
    public TMPro.TextMeshProUGUI discriptionText;
    //卡牌的费用
    public int cost;
    //卡牌上的费用显示
    public TextMeshProUGUI costText;
    //获取该卡牌上的recttransform组件
    public RectTransform rtTransform;
    //获取CanvasGroup组件
    public CanvasGroup canvasGroup;
    //获取Canvas
    public Canvas canvas;
    //获取打出手牌时位置
    public Vector2 startPos;
    public bool isDrag = false;
    //获取hover卡牌的位置
    public Vector2 hoverPos;
    //是否可以使用
    public bool canBeUse = true;
    //是否被使用
    public bool isUsed = false;
    //卡牌初始颜色
    public Color startColor;
    //是否被选中弃牌
    public bool isDiscard = false;
    //释放范围
    public int releaseRange = -1;
    private void Awake()
    {
        startColor=this.GetComponent<Image>().color;
        //找到该物体下的costText
        if(this.transform.Find("cost") != null)
        {
            costText = this.transform.Find("cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
        }
        //找到该物体下的discriptionText
        if(this.transform.Find("Text (TMP) (1)") != null)
        {
            discriptionText = this.transform.Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>();
            //discriptionText.text = discription;
        }
    }
    public void Start()
    {
        rtTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void Update()
    {
        costText.text = cost.ToString();
    }
    #region 将鼠标放在卡牌上的效果
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        //减去和手牌区的相对位置
        //startPos = this.transform.position;
        //Debug.Log(this.name + " startPos: " + startPos);
        if (isDrag)
        {
            return;
        }
        hoverPos = this.GetComponent<RectTransform>().anchoredPosition;        
        // 将该卡牌的UI层级向上提高
        this.transform.SetAsLastSibling();
        this.transform.localScale = new Vector3(1.8f, 1.8f, 1);
        this.GetComponent<RectTransform>().DOAnchorPos(new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x,
            hoverPos.y + 30f), 0.1f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(isDrag)
        {
            return;
        }
        //将卡牌的UI层级恢复
        this.transform.SetAsFirstSibling();
        this.transform.localScale = new Vector3(1f, 1f, 1);
        this.GetComponent<RectTransform>().DOAnchorPos(new Vector2(this.GetComponent<RectTransform>().anchoredPosition.x,
            hoverPos.y), 0.1f);
    }
    #endregion
    #region 拖动卡牌的效果
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        //startPos = this.transform.position;
        canvasGroup.blocksRaycasts = false;
        Debug.Log("onbegindrag");
        //以玩家为中心n*n的范围使地块变色
        ChangeCellsColor();
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        rtTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //如果没有释放，回到初始位置
        if(!isUsed)
        {
            this.GetComponent<RectTransform>().DOAnchorPos(this.GetComponent<Card>().hoverPos, 0.5f);
        }
        canvasGroup.blocksRaycasts = true;
        this.transform.localScale=new Vector3(1f, 1f, 1);
        Debug.Log("enddrag");
        Invoke("EndDrag", 0.5f);
    }
    #endregion
    #region ����Ч��
    public virtual void CardFunc()
    {

    }
    #endregion
    public void EndDrag() { isDrag = false; }
    public void OnPointerClick(PointerEventData eventData)
    {
        //如果打开了弃牌UI，点击卡牌使其高亮
        if(CardManager.instance.isDiscardUI)
        {
            if(isDiscard)
            {
                this.GetComponent<Image>().color = startColor;
                DiscardPanel.instance.curDiscardNum -= 1;
                isDiscard = false;
            }
            else
            {
                this.GetComponent<Image>().color = Color.red;
                DiscardPanel.instance.curDiscardNum += 1;
                isDiscard = true;
            }
        }
    }
    //让地块变色
    private void ChangeCellsColor()
    {
        if (releaseRange != -1)
        {

            int leftAxis = PlayerController.instance.location.x - 3 > 0 ? PlayerController.instance.location.x - 3 : 0;
            int rightAxis = PlayerController.instance.location.x + 3 < 9 ? PlayerController.instance.location.x + 3 : 9;
            int upAxis = PlayerController.instance.location.y - 3 > 0 ? PlayerController.instance.location.y - 3 : 0;
            int downAxis = PlayerController.instance.location.y + 3 < 9 ? PlayerController.instance.location.y + 3 : 9;
            for (int i = leftAxis; i <= rightAxis; i++)
            {
                for (int j = upAxis; j <= downAxis; j++)
                {

                }
            }
        }
    }
}
