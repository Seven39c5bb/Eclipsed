using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;
public class FightUI : UIBase
{
    public static List<Card> cardList;//�����е���
    public static FightUI instance;
    public TextMeshProUGUI cardCount;
    public TextMeshProUGUI discardCount;
    public GameObject deckPanel;private bool deckPanelFlag = false;private Vector2 deckPanelStartPos;
    private void Awake()
    {
        cardCount=GameObject.Find("cardCount").GetComponent<TextMeshProUGUI>();
        discardCount=GameObject.Find("discardCount").GetComponent <TextMeshProUGUI>();
        deckPanel = GameObject.Find("deckPanel");deckPanelStartPos = deckPanel.GetComponent<RectTransform>().anchoredPosition;

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
    }
    private void onClickEndTurn(GameObject obj,PointerEventData eventData)
    {
        //测试：：切换到敌人回合
        FightManager.instance.ChangeType(FightType.Enemy);           
    }
    private void onClickCardDeck(GameObject obj, PointerEventData data)
    {
        //��ʾ������ʣ����
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
    public void InstantiateCard(int count,string cardName)
    {
        if(count > CardManager.cardDesk.Count)
        {
            count=CardManager.cardDesk.Count;
        }
        for(int i = 0; i < count; i++)//生成卡牌
        {
            //
            GameObject obj=Instantiate(Resources.Load("Prefabs/Card/"+cardName),GameObject.Find("handCardArea").transform) as GameObject;//test ֻ����up
            //Debug.Log("instantiate A card");
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(GameObject.Find("cardDesk").GetComponent<RectTransform>().anchoredPosition.x,
                GameObject.Find("cardDesk").GetComponent<RectTransform>().anchoredPosition.y+100f);
            var objCard=obj.GetComponent<Card>();
            cardList.Add(objCard);
        }
        OnUpdateCardsPos();
    }
    public void OnUpdateCardsPos()
    {
        float offset=400f/cardList.Count;
        Vector2 startPos = new Vector2(-cardList.Count / 2f * offset + offset * 0.5f, 0);
        for(int i = 0;i < cardList.Count;i++)
        {
            cardList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.7f);
            startPos.x += offset;
        }
    }
}
