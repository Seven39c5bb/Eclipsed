using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTemplate : MonoBehaviour,IPointerClickHandler
{
    //卡牌——名字(作为识别预制体中的卡牌名字)
    public string card_Name;
    //卡牌名字
    public TMPro.TextMeshProUGUI cardName;
    //卡牌描述
    public TMPro.TextMeshProUGUI cardDescription;
    //卡牌花费
    public TMPro.TextMeshProUGUI cardCost;
    //卡牌图片
    public Sprite cardImage;
    //卡牌类型图片
    public Image cardTypeImg;

    /*检测卡牌面板和三选一面板的类型，确定点击该卡牌模板的效果*/
    public void OnPointerClick(PointerEventData eventData)
    {
        //获取该物体的父物体

        //如果场景中只有CardListPanel
        if (this.transform.parent.GetComponent<DeckBoard>())
        {
            if(this.transform.parent.GetComponent<DeckBoard>().type==DeckBoard.boardType.copy)
            {
                AddCard();
            }
            else if(this.transform.parent.GetComponent<DeckBoard>().type==DeckBoard.boardType.delete)
            {
                DeleteCard();
            }
            Destroy(GameObject.Find("CardListPanel(Clone)").gameObject);
        }
        //如果场景中有OptionPanel
        if (this.transform.parent.GetComponent<OptionPanel>())
        {
            if (this.transform.parent.GetComponent<OptionPanel>().type == OptionPanel.panelType.delete)
            {
                DeleteCard();
            }
            else if (this.transform.parent.GetComponent<OptionPanel>().type == OptionPanel.panelType.add)
            {
                AddCard();
            }
            Destroy(GameObject.Find("OptionPanel(Clone)").gameObject);
        }
    }
    //删除该卡牌
    public void DeleteCard()
    {
        SaveManager.instance.jsonData.playerData.playerDeck.Remove(card_Name);
        Debug.Log("删除了卡牌"+card_Name);
    }
    //添加该卡牌到卡组
    public void AddCard()
    {
        SaveManager.instance.jsonData.playerData.playerDeck.Add(card_Name);
        Debug.Log("添加了卡牌"+card_Name);
    }
    
}
