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
        Debug.Log(eventData.pointerDrag.name);
        //��ȡ��ǰʣ��
        //������ò��������ء�
        
        GameObject curCard = eventData.pointerDrag;//��ȡ������ק�Ŀ��ƶ���
        usingCard=curCard; 
        if (curCard.name.Contains("up"))
        {
            //��ȡ�������ϸÿ���cost
            Slider slider=curCard.GetComponentInChildren<Slider>();
            int value = (int)slider.value;
            //���curCost�����ͷţ�����
            if (costManager.instance.curCost < value) {
                curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);
                Debug.Log("no more cost");
                return; 
            }
            curCard.GetComponent<up>().MoveUp();
            //�ƶ������ƶѶ�����Ȼ������
            curCard.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.5f);
            Invoke("DestroyCard", 0.5f);
        }
        if (curCard.name == "down")
        {
            curCard.GetComponent<down>().MoveDown();
        }
    }
    public void DestroyCard() {CardManager.instance.discardDesk.Add(usingCard.GetComponent<Card>()); usingCard.SetActive(false);  }
}
