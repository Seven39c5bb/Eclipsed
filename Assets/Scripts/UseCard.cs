using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseCard : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name);
        //获取当前剩余
        //如果费用不够，返回。
        
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        if (curCard.name == "up")
        {
            curCard.GetComponent<up>().MoveUp();
        }
        if (curCard.name == "down")
        {
            curCard.GetComponent<down>().MoveDown();
        }
    }
}
