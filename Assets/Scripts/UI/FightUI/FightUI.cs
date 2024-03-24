using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;
public class FightUI : UIBase
{
    public static List<Card> cardList;//手牌中的牌
    public static FightUI instance;
    public TextMeshProUGUI cardCount;
    public TextMeshProUGUI discardCount;
    public TextMeshProUGUI health;
    public GameObject deckPanel;private bool deckPanelFlag = false;private Vector2 deckPanelStartPos;
    private void Awake()
    {
        cardCount=GameObject.Find("cardCount").GetComponent<TextMeshProUGUI>();
        discardCount=GameObject.Find("discardCount").GetComponent <TextMeshProUGUI>();
        deckPanel = GameObject.Find("deckPanel");deckPanelStartPos = deckPanel.GetComponent<RectTransform>().anchoredPosition;
        health = GameObject.Find("playerHealth").GetComponent<TextMeshProUGUI>();

        cardList = new List<Card>();
        instance = this;
        //战斗初始化  
        Register("endTurnButton").onClick = onClickEndTurn;
        Register("cardDesk").onClick = onClickCardDeck;
    }

    private void Start()
    {
        FightManager.instance.ChangeType(FightType.Init);
    }
    private void Update()
    {
        cardCount.text=CardManager.cardDesk.Count.ToString();
        discardCount.text=CardManager.discardDesk.Count.ToString();

        //health
        health.text = PlayerController.instance.HP.ToString() + "/" + PlayerController.instance.MaxHp.ToString();
    }
    private void onClickEndTurn(GameObject obj,PointerEventData eventData)
    {
        //测试：：切换到敌人回合
        FightManager.instance.ChangeType(FightType.Enemy);           
    }
    private void onClickCardDeck(GameObject obj, PointerEventData data)
    {
        //判断是否显示
        if(!deckPanelFlag)
        {
            deckPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(deckPanelStartPos.x - 105, deckPanelStartPos.y), 0.7f);
            deckPanelFlag = true;
        }
        else
        {
            deckPanel.GetComponent<RectTransform>().DOAnchorPos(deckPanelStartPos, 0.7f);
            deckPanelFlag = false;
        }
        
    }
    public void InstantiateCard(int count,string cardName)//this count meaningless
    {
        if(count > CardManager.cardDesk.Count)
        {
            count=CardManager.cardDesk.Count;
        }
        if (cardName.Contains("("))
        {
            int index = cardName.IndexOf("(");
            cardName=cardName.Substring(0, index);
        }
        for(int i = 0; i < count; i++)//生成卡牌
        {
            //
            Debug.Log("cardName you want to instantiate is :"+ cardName);
            GameObject obj=Instantiate(Resources.Load("Prefabs/Card/"+cardName),GameObject.Find("handCardArea").transform) as GameObject;//test ֻ����up
            //Debug.Log("instantiate A card");
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(GameObject.Find("cardDesk").GetComponent<RectTransform>().anchoredPosition.x,
            GameObject.Find("cardDesk").GetComponent<RectTransform>().anchoredPosition.y+100f);
            cardList.Add(obj.GetComponent<Card>());
        }
        OnUpdateCardsPos();
    }
    public void OnUpdateCardsPos()
    {
        float offset=400f/cardList.Count;
        Vector2 startPos = new Vector2(-cardList.Count / 2f * offset + offset * 0.5f, 0);
        for(int i = 0;i < cardList.Count;i++)
        {
            if (cardList[i] != null)
            {
                cardList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.7f);
                startPos.x += offset;
            }
            
        }
    }
}
