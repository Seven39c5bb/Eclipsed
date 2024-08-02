using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DiscardPanel : MonoBehaviour
{
    public static DiscardPanel dis_instance;
    public static DiscardPanel instance
    {
        get
        {
            if (dis_instance == null)
            {
                dis_instance = FindObjectOfType<DiscardPanel>();
            }
            return dis_instance;
        }
    }
    //弃牌数量
    public int disCardNum;
    public int curDiscardNum = 0;
    public bool canBeConfirmed;
    //文字提示
    public TextMeshProUGUI textTips;
    //按下确认键
    private void Update()
    {
        //如果没有足够可以弃的卡牌，关闭UI
        if (CardManager.instance.handCards[disCardNum-1]==null 
            ||CardManager.instance.handCards[disCardNum-1].GetComponent<Card>())
        {
            curDiscardNum = 0;
            this.gameObject.SetActive(false);
            CardManager.instance.isDiscardUI = false;
        }
        //如果没有选够足够的卡牌，不关闭UI
        if (curDiscardNum < disCardNum)
        {
            textTips.text = "请选择" + (disCardNum - curDiscardNum).ToString() + "张卡牌";
            canBeConfirmed = false;
        }
        else if(curDiscardNum == disCardNum)
        {
            textTips.text = "点击确认";
            canBeConfirmed = true;
        }
        else
        {
            textTips.text = "多选了";
            canBeConfirmed = false;
        }
    }
    public void Confirm()
    {
        if (!canBeConfirmed)
        {
            return;
        }
        //获取选中的卡牌，并弃掉这些卡牌 
        //遍历手牌，找到选中的卡牌
        foreach (Card card in CardManager.instance.handCards)
        {
            if (card.GetComponent<Card>().isDiscard)
            {
                CardManager.instance.Discard(card);
            }
        }
        curDiscardNum = 0;
        this.gameObject.SetActive(false);
        CardManager.instance.isDiscardUI = false;
    }
}
