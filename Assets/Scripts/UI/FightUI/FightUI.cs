using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class FightUI : UIBase
{
    public static List<Card> cardList;//手牌中的牌
    public static FightUI instance;
    public TextMeshProUGUI cardCount;
    public TextMeshProUGUI discardCount;
    public TextMeshProUGUI health;
    public GameObject deckPanel;private bool deckPanelFlag = false;private Vector2 deckPanelStartPos;
    //是否在敌人回合
    public bool isEnemyTurn = false;
    //test
    public GameObject deckViewer;
    Dictionary<string, int> cardDic = new Dictionary<string, int>();
    List<CardBoard> boardList;
    //test
    private void Awake()
    {
        cardCount=GameObject.Find("cardCount").GetComponent<TextMeshProUGUI>();
        discardCount=GameObject.Find("discardCount").GetComponent <TextMeshProUGUI>();
        deckPanel = GameObject.Find("Scroll View");deckPanelStartPos = deckPanel.GetComponent<RectTransform>().anchoredPosition;
        health = GameObject.Find("playerHealth").GetComponent<TextMeshProUGUI>();


        cardList = new List<Card>();
        cardDic=new Dictionary<string, int>();
        boardList = new List<CardBoard>();
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
        //card and discardcard
        cardCount.text=CardManager.cardDesk.Count.ToString();
        discardCount.text=CardManager.discardDesk.Count.ToString();

        //health
        health.text = PlayerController.instance.HP.ToString() + "/" + PlayerController.instance.MaxHp.ToString();    
    }
    private void onClickEndTurn(GameObject obj,PointerEventData eventData)
    {
        //测试：：切换到敌人回合
        if (!isEnemyTurn)
        {
            FightManager.instance.ChangeType(FightType.Enemy);   
        }
                
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
                Tween moveCard = cardList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.7f);
                moveCard.OnComplete(() =>
                {
                    for (int i = 0; i < cardList.Count; i++)//遍历手牌中的卡，将初始位置设定为当前所在位置
                    {
                        cardList[i].startPos = cardList[i].transform.position;
                        //Debug.Log(cardList[i].name+"now startPos :"+cardList[i].transform.position);
                    }                                           
                });
                moveCard.Play();
                startPos.x += offset;
                
            }
            
        }
    }


    public void InitDeckPanel()
    {
        for (int i = 0; i < CardManager.cardDesk.Count; i++)
        {
            cardDic[CardManager.cardDesk[i]] = 0;//初始化卡组为0
        }
        Vector3 offset = new Vector3(0, -25, 0);
        int times = 0;
        foreach(var ele in cardDic)//初始化生成cardBoard
        {
            GameObject cardBoard = Instantiate(Resources.Load("Prefabs/UI/cardBoard"), GameObject.Find("Content").transform) as GameObject;
            //往下移动offset
            cardBoard.transform.position += offset / GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor * times;
            cardBoard.GetComponent<CardBoard>().cardNameText.text = ele.Key;
            boardList.Add(cardBoard.GetComponent<CardBoard>());
            times++;
        }        
    }
    public void UpdateDeckPanel()
    {
        //test
        //遍历卡组，每遍历到一次该卡，该卡数量++
        for (int i = 0; i < CardManager.cardDesk.Count; i++)
        {
            cardDic[CardManager.cardDesk[i]] = 0;//初始化卡组为0
        }
        

        for (int i = 0;i<CardManager.cardDesk.Count ; i++)
        {
            cardDic[CardManager.cardDesk[i]]++;
        }
        foreach(var ele in cardDic)
        {
            //实例化一个cardBoard，调整cardboard位置
            GameObject cardBoard = Resources.Load<GameObject>("Prefabs/UI/cardBoard");

            //从预制体中获取卡牌费用
            GameObject obj = Resources.Load<GameObject>("Prefabs/Card/" + ele.Key);
            //Debug.Log(ele.Key);
            //遍历board列表
            foreach(var cboard in boardList)
            {
                //Debug.Log("cboard.cardnametxt:"+ cboard.cardNameText.text);
                if(cboard.cardNameText.text == ele.Key)
                {
                    //Debug.Log("yes");
                    cboard.costText.text = obj.GetComponent<Card>().cost.ToString();
                    //获取名字
                    cboard.cardNameText.text = ele.Key;
                    //获取费用
                    cboard.cardNumTxt.text = ele.Value.ToString();
                }
            }
            

        //test
        }
    }
}
