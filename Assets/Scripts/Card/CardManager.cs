using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static List<string> cardDesk;
    public static List<string> discardDesk;
    //test
    public List<string> d_discardDesk;
    //test
    public List<string> handCards;

    //
    public GameConfig gameConfig;
    private void Awake()    
    {
        c_instance = this;
        cardDesk = new List<string>();
        discardDesk = new List<string>();
        //��text�м���������Ϣ
        //......;0

        #region gameConfig 从json文档中读取卡组信息
        cardDesk=new List<string>(SaveManager.instance.jsonData.playerData.playerDeck);
        #endregion
        
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
}
