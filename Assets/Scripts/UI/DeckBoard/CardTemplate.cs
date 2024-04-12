using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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

    /*检测卡牌面板和三选一面板的类型，确定点击该卡牌模板的效果*/
    public void OnPointerClick(PointerEventData eventData)
    {
        //如果场景中有CardListPanel
        if (FindObjectOfType<DeckBoard>())
        {
            if(FindObjectOfType<DeckBoard>().type==DeckBoard.boardType.copy)
            {
                AddCard();
            }
            else if(FindObjectOfType<DeckBoard>().type==DeckBoard.boardType.delete)
            {
                DeleteCard();
            }
            Destroy(GameObject.Find("CardListPanel(Clone)").gameObject);
        }
        //如果场景中有OptionPanel
        if (FindObjectOfType<OptionPanel>())
        {
            if (FindObjectOfType<OptionPanel>().type == OptionPanel.panelType.delete)
            {
                DeleteCard();
            }
            else if (FindObjectOfType<OptionPanel>().type == OptionPanel.panelType.add)
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
    }
    //添加该卡牌到卡组
    public void AddCard()
    {
        SaveManager.instance.jsonData.playerData.playerDeck.Add(card_Name);
    }
    
}
