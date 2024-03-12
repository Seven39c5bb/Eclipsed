using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
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
    private RectTransform rtTransform;
    //��ȡCanvasGroup���
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
