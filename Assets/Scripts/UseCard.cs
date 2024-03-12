using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseCard : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name);
        //��ȡ��ǰʣ��
        //������ò��������ء�
        
        GameObject curCard = eventData.pointerDrag;//��ȡ������ק�Ŀ��ƶ���
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
