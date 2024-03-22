using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UseCard : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name);
        //获取当前剩余
        //如果费用不够，返回。
        
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        if (curCard.name.Contains("up"))
        {
            //获取滑动条上该卡牌cost
            Slider slider=curCard.GetComponentInChildren<Slider>();
            int value = (int)slider.value;
            //如果curCost不够释放，返回
            if (costManager.curCost < value) {
                curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);
                Debug.Log("no more cost");
                return; 
            }
            curCard.GetComponent<up>().MoveUp();
        }
        if (curCard.name == "down")
        {
            curCard.GetComponent<down>().MoveDown();
        }
    }
}
