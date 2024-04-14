using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //商人立绘
    public Sprite salemanSprite;
    //商店物品列表
    public List<ShopItem> shopItems;
    //指骨卡牌购买面板
    public GameObject PBItemPanel;
    //金币卡牌购买面板
    public GameObject CardItemPanel;
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
        InitShop();
    }
    //初始化商店
    public void InitShop()
    {
        //初始化商店物品
        shopItems = new List<ShopItem>();
        /*
         * 从上到下为4张仪式牌，中间3张普通牌，2张稀有牌，1张传说牌，底层为删除卡牌和回复
        */
        //找到指骨卡牌牌池，从中添加4张卡牌

        //找到Common卡牌牌池，从中添加3张卡牌

        string[] commonCards = Resources.Load<TextAsset>("TextAssets/CardPool/Common").text.Split(',');
        List<int> usedIndexes = new List<int>(); // 用于存储已使用的索引
        //从牌池中随机挑选3张卡牌
        //加载3张卡牌模板挂在该物体上
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, commonCards.Length); // 随机生成索引
            } while (usedIndexes.Contains(randomIndex)); // 如果已使用的索引中包含该索引，则重新生成
            usedIndexes.Add(randomIndex); // 将索引添加到已使用列表中

            GameObject card = Instantiate(Resources.Load("Prefabs/ShopItem/ShopItem"), CardItemPanel.transform) as GameObject;
            ShopItem cardTemplate = card.GetComponent<ShopItem>();
            GameObject cardMes = Resources.Load<GameObject>("Prefabs/Card/" + commonCards[randomIndex]);
            //获取cardMes上信息
            if (cardMes != null)
            {
                cardTemplate.card_Name = cardMes.name;
                //Debug.Log(cardMes.transform.GetChild(0).name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
            }
        }
        //找到Rare卡牌牌池，从中添加2张卡牌

        //找到Legend卡牌牌池，从中添加1张卡牌

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
