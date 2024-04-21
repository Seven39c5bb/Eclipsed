using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UseCard : MonoBehaviour,IDropHandler
{
    public GameObject usingCard;    
    public static UseCard UseCard_Instance;
    public static UseCard instance
    {
        get
        {
            if (UseCard_Instance == null)
            {
                UseCard_Instance = GameObject.FindObjectOfType<UseCard>();
            }
            return UseCard_Instance;
        }
    }
    private void Awake()
    {
        UseCard_Instance = this;
    }
    private void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        //��ȡ��ǰʣ��
        //������ò��������ء�
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        if (curCard.GetComponent<Card>()==null ) { return; }
        usingCard = curCard;


        //��ȡ�������ϸÿ���cost
        //���curCost�����ͷţ�����

        if (costManager.instance.curCost < curCard.GetComponent<Card>().cost)
            
        {
            Debug.Log("curcardcost: " + curCard.GetComponent<Card>().cost.ToString());
            curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);            
            Debug.Log("no more cost");
            return;
        }
        curCard.GetComponent<Card>().isUsed = true;
        

        //检测所有player上的buff，如果有buff触发条件为使用卡牌，则触发buff
        foreach(var buff in PlayerController.instance.buffList)
        {
            buff.OnUseCard(curCard.GetComponent<Card>());
        }

        if (curCard.GetComponent<Card>().isUsed == false) { return; }
        curCard.GetComponent<Card>().CardFunc();
        //将使用的牌移至弃牌堆
        //如果不是仪式卡
        if (curCard.GetComponent<Card>().type != Card.cardType.rites)
        {
            Discard(curCard);
        }
        else
        {
            FightUI.cardList.Remove(curCard.GetComponent<Card>());
            Destroy(curCard);
        }

    }

    public void Discard(GameObject curCard)
    {
        FightUI.cardList.Remove(curCard.GetComponent<Card>());
        FightUI.instance.OnUpdateCardsPos();
        CardManager.discardDesk.Add(curCard.GetComponent<Card>().name);
        curCard.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.5f).OnComplete(() =>
        {
            Destroy(curCard);
        });
    }

    //public void DestroyCard() {CardManager.discardDesk.Add(usingCard.name); usingCard.SetActive(false);  }
    //将所有手牌移至弃牌堆
    public void RemoveAllCards()
    {
        //Debug.Log("touch!");//test

        for (int i=0;i<FightUI.cardList.Count;i++)
        {            
            FightUI.cardList.Remove(FightUI.cardList[i]); 
        }
        Transform handCardArea = GameObject.Find("handCardArea").GetComponent<Transform>();
        CardManager.instance.handCards.Clear();
        for(int i=0; i < handCardArea.childCount; i++)
        {
            Transform childT = handCardArea.GetChild(i);
            Destroy(childT.gameObject);
        }
    }
}
