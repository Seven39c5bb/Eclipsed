using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : UIBase,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    //ö�ٿ��Ƶ�����
    public enum cardType
    {
        action,skill,trap
    }
    //���Ƶ�����
    public cardType type;
    //��������
    public string discription;
    //���Ƶķ���
    public int cost;
    //��ȡ�ÿ����ϵ�recttransform���
    public RectTransform rtTransform;
    //��ȡCanvasGroup���
    public CanvasGroup canvasGroup;
    //��ȡCanvas
    public Canvas canvas;
    public void Start()
    {
        rtTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    public void Update()
    {
        
    }
    #region �������ڿ����ϵ�Ч��
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f, 1);
    }
    #endregion
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        Debug.Log("onbegindrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rtTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //���û���ͷţ��ص���ʼλ��
        canvasGroup.blocksRaycasts = true;
        Debug.Log("enddrag");
    }

    
}
