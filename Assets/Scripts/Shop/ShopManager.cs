using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    //是否删除卡牌和是否回血
    public bool isDeleteCard;
    public bool isHeal;
    //信息面板
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI fingerBoneText;
    public TextMeshProUGUI CardDeckText;
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
        healthText = GameObject.Find("healthText").GetComponent<TextMeshProUGUI>();
        coinText = GameObject.Find("coinText").GetComponent<TextMeshProUGUI>();
        fingerBoneText = GameObject.Find("fingerBoneText").GetComponent<TextMeshProUGUI>();
        CardDeckText = GameObject.Find("CardDeckText").GetComponent<TextMeshProUGUI>();

        isDeleteCard = false;
        isHeal = false;
    }
    void Update()
    {
        healthText.text = SaveManager.instance.jsonData.playerData.HP.ToString() + "/" + SaveManager.instance.jsonData.playerData.MaxHP.ToString();
        coinText.text = SaveManager.instance.jsonData.playerData.coin.ToString();
        fingerBoneText.text = SaveManager.instance.jsonData.playerData.fingerBone.ToString();
        CardDeckText.text = SaveManager.instance.jsonData.playerData.playerDeck.Count.ToString();
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
        UpdatePBCard();
        //找到Common卡牌牌池，从中添加3张卡牌
        UpdateCardFromPool("Common",3);
        //找到Rare卡牌牌池，从中添加2张卡牌
        UpdateCardFromPool("Rare",2);
        //找到Legend卡牌牌池，从中添加1张卡牌
        UpdateCardFromPool("Legend",1);
    }
    private void UpdatePBCard()
    {
        string[] Cards = Resources.Load<TextAsset>("TextAssets/CardPool/PBCards").text.Split(',');
        List<int> usedIndexes = new List<int>(); // 用于存储已使用的索引
        //从牌池中随机挑选3张卡牌
        //加载3张卡牌模板挂在该物体上
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, Cards.Length); // 随机生成索引
            } while (usedIndexes.Contains(randomIndex)); // 如果已使用的索引中包含该索引，则重新生成
            usedIndexes.Add(randomIndex); // 将索引添加到已使用列表中

            GameObject card = Instantiate(Resources.Load("Prefabs/ShopItem/ShopItem"), PBItemPanel.transform) as GameObject;
            ShopItem cardTemplate = card.GetComponent<ShopItem>();
            GameObject cardMes = Resources.Load<GameObject>("Prefabs/Card/" + Cards[randomIndex]);
            Debug.Log(Cards[randomIndex]);
            //获取cardMes上信息
            if (cardMes != null)
            {
                cardTemplate.card_Name = cardMes.name;
                Debug.Log(cardMes.name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardCost.text = cardMes.GetComponent<Card>().cost.ToString();
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
                cardTemplate.buyTimes = 1;
                cardTemplate.buyType= ShopItem.buyItem.fingerbone;
                //根据指骨牌购买次数设置价格
                cardTemplate.price = 1 + SaveManager.instance.jsonData.playerData.fbCardBuyTimes * 2;
                cardTemplate.cardPrice.text = cardTemplate.price.ToString();

                switch (cardMes.GetComponent<Card>().rare)
                {
                    case Card.rareType.common:
                        cardTemplate.cardName.color = Color.white;
                        break;
                    case Card.rareType.rare:
                        cardTemplate.cardName.color = Color.blue;
                        break;
                    case Card.rareType.legend:
                        cardTemplate.cardName.color = new Color(1, (float)(148 / 255), (float)(68 / 255));
                        break;
                }
                Sprite[] sprites = Resources.LoadAll<Sprite>("Pictures/CardImg/cardTypeImg");
                switch (cardMes.GetComponent<Card>().type)
                {
                    case Card.cardType.action:
                        cardTemplate.cardTypeImg.sprite = sprites[0];
                        break;
                    case Card.cardType.skill:
                        cardTemplate.cardTypeImg.sprite = sprites[4];
                        break;
                    case Card.cardType.rites:
                        cardTemplate.cardTypeImg.sprite = sprites[3];
                        break;
                    case Card.cardType.trap:
                        cardTemplate.cardTypeImg.sprite = sprites[5];
                        break;
                }

                //更换卡面
                if (Resources.Load("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName) != null)
                {
                    cardTemplate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName);                
                }
            }
        }
    }
    private void UpdateCardFromPool(string cardPool,int num)
    {
        string[] Cards = Resources.Load<TextAsset>("TextAssets/CardPool/"+cardPool).text.Split(',');
        Debug.Log(Resources.Load<TextAsset>("TextAssets/CardPool/" + cardPool).text);
        List<int> usedIndexes = new List<int>(); // 用于存储已使用的索引
        //从牌池中随机挑选3张卡牌
        //加载3张卡牌模板挂在该物体上
        for (int i = 0; i < num; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, Cards.Length); // 随机生成索引
            } while (usedIndexes.Contains(randomIndex)); // 如果已使用的索引中包含该索引，则重新生成
            usedIndexes.Add(randomIndex); // 将索引添加到已使用列表中

            GameObject card = Instantiate(Resources.Load("Prefabs/ShopItem/ShopItem"), CardItemPanel.transform) as GameObject;
            ShopItem cardTemplate = card.GetComponent<ShopItem>();
            GameObject cardMes = Resources.Load<GameObject>("Prefabs/Card/" + Cards[randomIndex]);
            Debug.Log(Cards[randomIndex]);
            //获取cardMes上信息
            if (cardMes != null)
            {
                cardTemplate.card_Name = cardMes.name;
                cardTemplate.cardCost.text = cardMes.GetComponent<Card>().cost.ToString();
                Debug.Log(cardMes.name);
                //Debug.Log(cardMes.transform.GetChild(0).name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
                cardTemplate.buyTimes = 1;
                //根据卡牌稀有度设置价格
                Debug.Log(cardMes.GetComponent<Card>().rare);
                switch (cardMes.GetComponent<Card>().rare)
                {
                    case Card.rareType.common:
                        cardTemplate.price = 50;
                        cardTemplate.cardPrice.text = "50";
                        break;
                    case Card.rareType.rare:
                        cardTemplate.price = 75;
                        cardTemplate.cardPrice.text = "75";
                        break;
                    case Card.rareType.legend:
                        cardTemplate.price = 125;
                        cardTemplate.cardPrice.text = "125";
                        break;
                }
                
                switch (cardMes.GetComponent<Card>().rare)
                {
                    case Card.rareType.common:
                        cardTemplate.cardName.color = Color.white;
                        break;
                    case Card.rareType.rare:
                        cardTemplate.cardName.color = Color.blue;
                        break;
                    case Card.rareType.legend:
                        cardTemplate.cardName.color = new Color(1, (float)(148 / 255), (float)(68 / 255));
                        break;
                }
                Sprite[] sprites = Resources.LoadAll<Sprite>("Pictures/CardImg/cardTypeImg");
                switch (cardMes.GetComponent<Card>().type)
                {
                    case Card.cardType.action:
                        cardTemplate.cardTypeImg.sprite = sprites[0];
                        break;
                    case Card.cardType.skill:
                        cardTemplate.cardTypeImg.sprite = sprites[4];
                        break;
                    case Card.cardType.rites:
                        cardTemplate.cardTypeImg.sprite = sprites[3];
                        break;
                    case Card.cardType.trap:
                        cardTemplate.cardTypeImg.sprite = sprites[5];
                        break;
                }

                //更换卡面
                if (Resources.Load("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName) != null)
                {
                    cardTemplate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName);
                }
            }
        }
    }
    //打开删卡面板
    public void OpenDeletePanel()
    {
        if(SaveManager.instance.jsonData.playerData.coin<75 || isDeleteCard)
        {
            return;
        }
        GameObject deletePanel = Instantiate(Resources.Load("Prefabs/UI/CardListPanel"), GameObject.Find("Canvas").transform) as GameObject;
        FindObjectOfType<DeckBoard>().type=DeckBoard.boardType.delete;
        SaveManager.instance.jsonData.playerData.coin -= 75;
        isDeleteCard = true;
        GameObject.Find("deleteCard").GetComponent<Image>().color = Color.gray;
    }
    //回血
    public void Heal()
    {
        if(SaveManager.instance.jsonData.playerData.coin<50 || isHeal)
        {
            return;
        }
        SaveManager.instance.jsonData.playerData.HP += 30;
        if (SaveManager.instance.jsonData.playerData.HP > SaveManager.instance.jsonData.playerData.MaxHP)//确保生命值不超过最大生命值
        {
            SaveManager.instance.jsonData.playerData.HP = SaveManager.instance.jsonData.playerData.MaxHP;
        }
        SaveManager.instance.jsonData.playerData.coin -= 50;
        isHeal = true;
        GameObject.Find("Heal").GetComponent<Image>().color = Color.gray;
    }
    //展示卡组
    public void ShowDeck()
    {
        GameObject deletePanel = Instantiate(Resources.Load("Prefabs/UI/CardListPanel"), GameObject.Find("Canvas").transform) as GameObject;
        FindObjectOfType<DeckBoard>().type = DeckBoard.boardType.showOnly;
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
