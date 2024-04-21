using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    //物品名称
    public string itemName;
    //用什么购买
    public enum buyItem { coin,fingerbone}
    public enum itemType {fingerBone,Common,Rare,Legend}
    public itemType type;
    public buyItem buyType;
    //物品价格
    public int price;
    //卡牌——名字(作为识别预制体中的卡牌名字)
    public string card_Name;
    //卡牌名字
    public TMPro.TextMeshProUGUI cardName;
    //卡牌描述
    public TMPro.TextMeshProUGUI cardDescription;
    //卡牌花费
    public TMPro.TextMeshProUGUI cardCost;
    //卡牌价格文字
    public TMPro.TextMeshProUGUI cardPrice;
    //卡牌图片
    public Sprite cardImage;
    //物品可购买次数
    public int buyTimes;
    //如果购买次数等于0，不可再次购买
    //购买
    public void BuyCard()
    {
        //改变商品物品颜色为灰色，并标记已购买
        //扣钱
        if (this.buyType == buyItem.coin)
        {
            if (SaveManager.instance.jsonData.playerData.coin >= price && buyTimes>0)
            {
                SaveManager.instance.jsonData.playerData.coin-=price;
                buyTimes -= 1;
                //将卡牌放入卡组
                SaveManager.instance.jsonData.playerData.playerDeck.Add(this.card_Name);
                SaveManager.instance.Save();
            }
            else { Debug.Log("金币不足"); }/*提示金币不足*/
        }
        else
        {
            if (SaveManager.instance.jsonData.playerData.fingerBone >= price)
            { SaveManager.instance.jsonData.playerData.fingerBone -= price;
              //将卡牌放入卡组
              SaveManager.instance.jsonData.playerData.playerDeck.Add(this.card_Name);
              SaveManager.instance.jsonData.playerData.fbCardBuyTimes += 1;
              SaveManager.instance.Save();             
            }
            else { Debug.Log("指骨不足"); }/*提示骨头不足*/
        }
        if(buyTimes<=0)
        {
            this.GetComponent<Image>().color=Color.gray;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //提示是否购买
        Debug.Log("buy card");
        BuyCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buyTimes > 0)
        {
            this.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(buyTimes > 0)
        {
            this.GetComponent<Image>().color = Color.white;
        }
    }
}
