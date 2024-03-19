using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseCard : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name);
        //��ȡ��ǰʣ��
        //������ò��������ء�
        
        GameObject curCard = eventData.pointerDrag;//��ȡ������ק�Ŀ��ƶ���
        if (curCard.name.Contains("up"))
        {
            //��ȡ�������ϸÿ���cost
            Slider slider=curCard.GetComponentInChildren<Slider>();
            int value = (int)slider.value;
            //���curCost�����ͷţ�����
            if (costManager.curCost < value) {
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
