using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FightUI : UIBase
{
    private List<Card> cardList;
    public static FightUI instance;
    private void Awake()
    {
        cardList = new List<Card>();
        instance = this;
        //战斗初始化  
        Register("endTurnButton").onClick = onClickEndTurn;
    }
    private void Start()
    {
        Debug.Log("test");
        FightManager.instance.ChangeType(FightType.Init);
    }
    private void onClickEndTurn(GameObject obj,PointerEventData eventData)
    {
        //测试：：切换到敌人回合
        FightManager.instance.ChangeType(FightType.Enemy);           
    }
    public void InstantiateCard(int count)
    {
        Debug.Log(CardManager.instance.cardDesk.Count);
        if(count > CardManager.instance.cardDesk.Count)
        {
            count=CardManager.instance.cardDesk.Count;
        }
        for(int i = 0; i < count; i++)//生成卡牌
        {
            GameObject obj=Instantiate(Resources.Load("Prefabs/Card/up"),GameObject.Find("handCardArea").transform) as GameObject;
            Debug.Log("instantiate A card");
            obj.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-100f);
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
