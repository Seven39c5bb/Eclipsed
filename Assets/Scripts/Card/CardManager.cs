using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public static List<string> cardDesk;
    public static List<string> discardDesk;
    public List<string> handCards;
    public int test = 1;

    //test 
    public GameConfig gameConfig;
    //test
    private void Awake()    
    {
        instance = this;
        cardDesk = new List<string>();
        discardDesk = new List<string>();
        //��text�м���������Ϣ
        //......;0

        #region gameConfig
        gameConfig = new GameConfig();
        gameConfig.Init();
        foreach(KeyValuePair<string,int> ele in gameConfig.cardDeckData)
        {
            string cardName=ele.Key;
            int cardCount=ele.Value;
            //GameObject card = Resources.Load("Prefabs/Card/" + cardName) as GameObject;
            for(int i = 0; i < cardCount; i++)///test 5 �Ļ�(int)cardCount
            {
                cardDesk.Add(cardName);
            }      
        }
        #endregion
        #region
        FightUI.instance.InitDeckPanel();
        #endregion
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
}
