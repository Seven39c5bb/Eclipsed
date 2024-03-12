using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    //枚举卡牌的种类
    public enum cardType
    {
        action,skill,trap
    }
    //卡牌的种类
    public cardType type;
    //卡牌描述
    public string discription;
    //卡牌的费用
    public int cost;
    //获取该卡牌上的recttransform组件
    private RectTransform rtTransform;
    //获取CanvasGroup组件
    private CanvasGroup canvasGroup;
    private void Start()
    {
        rtTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        Debug.Log("onbegindrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rtTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Debug.Log("enddrag");
    }
}
