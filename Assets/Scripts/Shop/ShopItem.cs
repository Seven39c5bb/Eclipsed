using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    //物品名称
    public string itemName;
    //用什么购买
    public enum buyItem { coin,fingerbone}
    public enum itemType { card,deleteCard}
    public itemType type;
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
                break;
            case itemType.deleteCard:
                //购买删除卡牌
                //打开删除卡牌的界面
                //删除卡牌
                break;
        }
    }
}
