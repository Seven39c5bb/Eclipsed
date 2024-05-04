using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buffBoard : MonoBehaviour, IPointerExitHandler
{
    public BuffBase buff;
    public UnityEngine.UI.Image buffImg;
    public stateBoard stateBoard;
    public bool isDetailed = true;
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDetailed) stateBoard.instance.HideTooltip();
    }
    void Start()
    {
        stateBoard = GameObject.Find("stateBoard").GetComponent<stateBoard>();
        buffImg = GetComponent<UnityEngine.UI.Image>();
        switch (buff.buffImgType)
        {
            case BuffBase.BuffImgType.Damage:
                buffImg.sprite = Resources.Load<Sprite>("Pictures/UI/DamageBuff");
                break;
            case BuffBase.BuffImgType.Defense:
                buffImg.sprite = Resources.Load<Sprite>("Pictures/UI/DefenseBuff");
                break;
            case BuffBase.BuffImgType.HP:
                buffImg.sprite = Resources.Load<Sprite>("Pictures/UI/HPBuff");
                break;
            case BuffBase.BuffImgType.Action:
                buffImg.sprite = Resources.Load<Sprite>("Pictures/UI/ActionBuff");
                break;
            case BuffBase.BuffImgType.Cost:
                buffImg.sprite = Resources.Load<Sprite>("Pictures/UI/CostBuff");
                break;
        }
        switch (buff.buffType)
        {
            case BuffBase.BuffType.Buff:
                buffImg.color = new Color(1, 1, 1, 1);//白色
                break;
            case BuffBase.BuffType.Debuff:
                buffImg.color = new Color(0, 0, 1, 1);//蓝色
                break;
        }
    }
    public void OnBuffEnter()
    {
        if (isDetailed) stateBoard.instance.ShowTooltip(buff.description, buff.buffNameCN);
    }
}
