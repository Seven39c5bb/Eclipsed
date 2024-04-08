using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //商人立绘
    public Sprite salemanSprite;
    //商店物品列表
    public List<ShopItem> shopItems;
    //单例
    public static ShopManager Shop_instance;
    public static ShopManager instance
    {
        get
        {
            if(Shop_instance== null)
            {
                Shop_instance=GameObject.FindObjectOfType<ShopManager>();
            }
            return Shop_instance;
        }
    }
    public List<Transform> slotTR;//总共有10个槽位
    //特殊卡牌购买次数
    public int specialCardBuyTimes;
    private void Awake()
    {
        Shop_instance = this;
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
    //
    //返回地图
    public void BackToAltas()
    {
        //保存json文件
        SaveManager.instance.Save();
        switch (SaveManager.instance.jsonData.mapData.backAtlasID)
        {
            case MapManager.AtlasID.Atlas_1:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_1");
                break;
            case MapManager.AtlasID.Atlas_2:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_2");
                break;
            case MapManager.AtlasID.Atlas_3:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_3");
                break;
            case MapManager.AtlasID.Atlas_4:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_4");
                break;
        }

    }
}
