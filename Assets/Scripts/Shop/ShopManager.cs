using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //商人立绘
    public Sprite salemanSprite;
    //商店物品列表
    public List<ShopItem> shopItems;
    public static ShopManager instance;
    public List<Transform> slotTR;//总共有10个槽位
    //特殊卡牌购买次数
    public int specialCardBuyTimes;
    private void Awake()
    {
        instance = this;
    }
    //初始化商店
    public void InitShop()
    {
        //初始化商店物品
        shopItems = new List<ShopItem>();
        /*
         * 从上到下为4张仪式牌，中间3张普通牌，3张稀有牌，底层为删除卡牌和回复
         */

    }
}
