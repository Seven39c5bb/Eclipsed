using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UseCard : MonoBehaviour,IDropHandler
{
    public GameObject usingCard;
    public static UseCard instance;
    private void Awake()
    {
        instance = this;
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
        curCard.GetComponent<Card>().CardFunc();
        curCard.GetComponent<Card>().isUsed = true;

        //将使用的牌移至弃牌堆
        Discard(curCard);

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
    public void RemoveAllCards()
    {
        //Debug.Log("touch!");//test

        for (int i=0;i<FightUI.cardList.Count;i++)
        {            
            FightUI.cardList.Remove(FightUI.cardList[i]); 
        }
        Transform handCardArea = GameObject.Find("handCardArea").GetComponent<Transform>();
        for(int i=0; i < handCardArea.childCount; i++)
        {
            Transform childT = handCardArea.GetChild(i);
            Destroy(childT.gameObject);
        }
    }
}
