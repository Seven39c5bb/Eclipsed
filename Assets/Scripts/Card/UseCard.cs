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
        //鑾峰彇褰撳墠鍓╀綑
        //濡傛灉璐圭敤涓嶅锛岃繑鍥炪��
        
        GameObject curCard = eventData.pointerDrag;//获取正在拖拽的卡牌对象
        usingCard=curCard; 
        if (curCard.name.Contains("up"))
        {
            //鑾峰彇婊戝姩鏉′笂璇ュ崱鐗宑ost
            Slider slider=curCard.GetComponentInChildren<Slider>();
            int value = (int)slider.value;
            //如果curCost不够释放，返回
            if (costManager.instance.curCost < value) {
                curCard.GetComponent<RectTransform>().DOMove(curCard.GetComponent<Card>().startPos, 0.5f);
                Debug.Log("no more cost");
                return; 
            }
            curCard.GetComponent<up>().MoveUp();
            //移动到弃牌堆动画，然后销毁
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
