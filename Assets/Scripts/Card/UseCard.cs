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
        //获取当前剩余
        //如果费用不够，返回。
        
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        usingCard=curCard; 


        //获取滑动条上该卡牌cost
        //如果curCost不够释放，返回

        if (costManager.instance.curCost < curCard.GetComponent<Card>().cost) {
            curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);
            Debug.Log("no more cost");
            return; 
        }
        curCard.GetComponent<Card>().CardFunc();


        //移动到弃牌堆动画，然后销毁
        FightUI.cardList.Remove(curCard.GetComponent<Card>());
        FightUI.instance.OnUpdateCardsPos();
        curCard.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.5f);
        Invoke("DestroyCard", 0.5f);
        
    }
    public void DestroyCard() {CardManager.discardDesk.Add(usingCard.GetComponent<Card>()); usingCard.SetActive(false);  }
}
