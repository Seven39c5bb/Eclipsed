using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    //物品名称
    public string itemName;
    //用什么购买
    public enum buyItem { coin,fingerbone}
    public enum itemType { card,deleteCard}
    public itemType type;
    public buyItem buyType;
    //物品价格
    public int price;
    //物品图标
    public Sprite itemSprite;
    //物品描述
    public string itemDescription;
    //物品可购买次数
    public int buyTimes;
    //如果购买次数等于0，不可再次购买
    //购买
    void BUY_THIS()
    {
        //当鼠标点击改物品时
        switch(this.type)
        {
            case itemType.card:
                //购买卡牌
                //将购买的卡牌放入卡组
                BuyCard();
                break;
            case itemType.deleteCard:
                //购买删除卡牌
                //打开删除卡牌的界面
                //删除卡牌
                break;
        }
    }
    public void BuyCard()
    {
        //改变商品物品颜色为灰色，并标记已购买
        //扣钱
        if (this.buyType == buyItem.coin)
        {
            if (SaveManager.instance.jsonData.playerData.coin >= price)
            {
                SaveManager.instance.jsonData.playerData.coin-=price;
            }
            else { Debug.Log("金币不足"); }/*提示金币不足*/
        }
        else
        {
            if (SaveManager.instance.jsonData.playerData.fingerBone >= price)
            { SaveManager.instance.jsonData.playerData.fingerBone -= price; }
            else { }/*提示骨头不足*/
        }
        //将卡牌放入卡组
        SaveManager.instance.jsonData.playerData.playerDeck.Add(this.name);
        SaveManager.instance.Save();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //提示是否购买
        Debug.Log("buy card");
        BuyCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
    }
}
