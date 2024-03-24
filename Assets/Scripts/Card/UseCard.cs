using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UseCard : MonoBehaviour,IDropHandler
{
    public GameObject usingCard;
    private void Start()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        //��ȡ��ǰʣ��
        //������ò��������ء�
        
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        usingCard=curCard; 


        //��ȡ�������ϸÿ���cost
        //���curCost�����ͷţ�����

        if (costManager.instance.curCost < curCard.GetComponent<Card>().cost) {
            curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);
            Debug.Log("no more cost");
            return; 
        }
        curCard.GetComponent<Card>().CardFunc();


        //�ƶ������ƶѶ�����Ȼ������
        FightUI.cardList.Remove(curCard.GetComponent<Card>());
        FightUI.instance.OnUpdateCardsPos();
        curCard.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.5f);
        Invoke("DestroyCard", 0.5f);
        
    }
    public void DestroyCard() {CardManager.discardDesk.Add(usingCard.GetComponent<Card>()); usingCard.SetActive(false);  }
}
