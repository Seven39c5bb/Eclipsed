﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public static CardManager c_instance;
    public static CardManager instance
    {
        get
        {
            if(c_instance == null)
            {
                c_instance=GameObject.FindObjectOfType<CardManager>();
            }
            return c_instance;
        }
    }
    [Header("卡组")]
    public static List<string> cardDesk;
    public static List<string> discardDesk;
    //test
    public List<string> d_discardDesk;
    //手牌
    [Header("手牌")]
    public List<Card> handCards=new List<Card>();
    public GameObject handCardArea;
    //初始抽牌数量
    public int drawNum = 5;
    //
    public GameConfig gameConfig;
    //是否打开弃牌UI
    public GameObject discardPanel;
    public bool isDiscardUI = false;
    [Header("开发者模式")]
    public bool DEVELOPE_MODE;
    private void Awake()    
    {
        c_instance = this;
        cardDesk = new List<string>();
        discardDesk = new List<string>();
        //��text�м���������Ϣ
        //......;0

        #region gameConfig 从json文档中读取卡组信息
        if(!DEVELOPE_MODE)
        cardDesk=new List<string>(SaveManager.instance?.jsonData.playerData.playerDeck);
        //test
        else
        {
            cardDesk = new List<string>();
            if (cardDesk.Count <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    cardDesk.Add("Snipe");
                }
            }
        }
        //test
        #endregion

        handCardArea = GameObject.Find("handCardArea");
        
    }
    //Start
    private void Start()
    {
        #region 初始化记牌器
        FightUI.instance.InitDeckPanel();
        #endregion
        //如果有存档，从存档中读取卡组信息
        //cardDeck=new List<string>(SaveManager.instance.jsonData.cardDeckData);
        //如果没有存档，读取初始卡组信息
    }
    private void Update()
    {
        //test
        //d_discardDesk = new List<string>(discardDesk);
        //让handCardArea的所有子物体放进handCards
        //如果手牌有变化，实时更新手牌


        //如果处于玩家回合， 如果手牌数量发生变化，实时更新手牌
        if (FightManager.instance.curFightType == FightType.Player)
        {
            if (handCardArea.transform.childCount != handCards.Count && handCardArea.transform.childCount>0)
            {
                handCards.Clear(); // 清空手牌列表
                for (int i = 0; i < handCardArea.transform.childCount; i++)
                {
                    Transform child = handCardArea.transform.GetChild(i);
                    if (child.GetComponent<Card>() != null)
                    {
                        handCards.Add(child.GetComponent<Card>());
                    }
                }
            }
        }

        //test
    }
    //抽卡
    public void Draw(int num)
    {
        for(int i=0;i<num;i++)
        {
            if(cardDesk.Count<=0)
            {
                UPdateDesk();
            }
            string drawCard = cardDesk[Random.Range(0, cardDesk.Count - 1)];
            //生成一张牌到手中
            FightUI.instance.InstantiateCard(1,drawCard);
            cardDesk.Remove(drawCard);
            //更新卡组卡牌显示
            FightUI.instance.UpdateDeckPanel();
        }
    }
    public void UPdateDesk()//更新牌组
    {
        cardDesk = new List<string>(discardDesk);
        discardDesk.Clear(); 
        //将弃牌堆中隐藏的卡牌全部清除
        GameObject handCardArea = GameObject.Find("handCardArea");
        Transform hcAreaTF= handCardArea.transform;    
    }
    //弃牌
    public void OpenDiscardUI(int num)
    {
        //打开弃牌UI
        if (!isDiscardUI)
        {
            discardPanel.SetActive(true);
            DiscardPanel.instance.disCardNum = num;
            DiscardPanel.instance.textTips.text = "请选择" + num + "张卡牌弃掉";
            isDiscardUI = true;
        }
    }
    public void Discard(Card card)
    {
        //播放弃牌动画
        //将卡牌放入弃牌堆
        Debug.Log(card.cardName);
        FightUI.cardList.Remove(card.GetComponent<Card>());
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
        }
        FightUI.instance.OnUpdateCardsPos();
        CardManager.discardDesk.Add(card.GetComponent<Card>().name);
        foreach(var buff in PlayerController.instance.buffList)
        {
            buff.OnDisCardCard(card);
        }
        card.GetComponent<RectTransform>().DOMove(card.transform.position + new Vector3(0, 200, 0), 1f).OnComplete(() =>
        {
            card.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.5f).OnComplete(() =>
            {
                Destroy(card.gameObject);
            });
        });
    }
}
