using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : UIBase,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
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
    public RectTransform rtTransform;
    //获取CanvasGroup组件
    public CanvasGroup canvasGroup;
    //获取Canvas
    public Canvas canvas;
    //获取打出手牌时位置
    public Vector3 startPos;
    private void Awake()
    {
        
    }
    public void Start()
    {
        rtTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void Update()
    {
        
    }
    #region 将鼠标放在卡牌上的效果
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f, 1);
    }
    #endregion
    #region ��ק���Ƶ�Ч��
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = this.transform.position;
        canvasGroup.blocksRaycasts = false;
        Debug.Log("onbegindrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rtTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //如果没有释放，回到初始位置
        canvasGroup.blocksRaycasts = true;
        Debug.Log("enddrag");
    }
    #endregion
    #region ����Ч��
    public virtual void CardFunc()
    {

    }
    #endregion

}
