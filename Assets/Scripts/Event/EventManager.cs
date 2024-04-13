using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager event_Instance;
    public static EventManager instance
    {
        get
        {
            if (event_Instance == null)
            {
                event_Instance = GameObject.FindObjectOfType<EventManager>();
            }
            return event_Instance;
        }
    }
    //事件名
    public TMPro.TextMeshProUGUI eventName;
    //事件描述
    public TMPro.TextMeshProUGUI eventDescription;
    //主角立绘
    public Image playerImg;
    //选项List
    public List<EventBase> eventList;
    //eventAsset
    public TextAsset eventAsset;
    private void Awake()
    {
        event_Instance = this;
        //加载eventList
        eventList = new List<EventBase>();

        //加载eventAsset
        //处理eventAsset
        HandleEventAsset();
        //对每个选项进行处理     
        for (int i = 0; i < eventList.Count; i++)
        {
            //生成一个选项按钮
            GameObject buttonObj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/option"), GameObject.Find("choicePanel").transform);
            //获取该button的子物体
            TextMeshProUGUI optionText = buttonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            optionText.text = eventList[i].optionDescription;
            //为button添加点击事件
            //判断该事件的功能
            string[] funcs= eventList[i].optionFunc.Split(',');
            string[] datas= eventList[i].optionData.Split(',');
            for(int j= 0;j < funcs.Length;j++)
            {
                #region 为button添加点击事件,判断该事件的功能
                if (funcs[j] == "ChangeHealth")
                {
                    string healNum = datas[j];
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => ChangeHealth(healNum, eventResult));
                }
                else if (funcs[j] == "ChangeCoin")
                {
                    string coinNum = datas[j];
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => ChangeCoin(coinNum,eventResult));
                }
                else if (funcs[j] == "OptionPanel")
                {
                    //传入数据
                    string panelType = datas[j];
                    string eventResult = eventList[i].optionResult;
                    string cardPool = datas[++j];
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => OpenOptionPanel(panelType, eventResult,cardPool));
                }
                else if (funcs[j] == "CardsPanel")
                {
                    string panelType = datas[j];
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => OpenCardsPanel(panelType, eventResult));
                }
                else if (funcs[j] == "ChangeFB")
                {
                    string FbNum = datas[j];
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => ChangeFB(FbNum, eventResult));
                }
                else if (funcs[j]=="CardAppend")
                {
                    string cardsData = datas[j];
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() => CardAppend(cardsData,eventResult));
                }
                #endregion
            }
        }
    }

    //处理eventAsset
    public void HandleEventAsset()
    {
        //从eventAsset中读取事件信息
        string[] eventParts = eventAsset.text.Split('：');
        //事件名字
        string eventName = eventParts[1];
        this.eventName.text = eventName;
        //事件描述
        string eventDescription = eventParts[3];
        this.eventDescription.text = eventDescription;
        //接下来每3行作为一个事件选项
        for (int i = 5; i < eventParts.Length; i += 8)
        {
            //事件选项描述
            string optionDescription = eventParts[i];
            //事件选项功能
            string optionFunc = eventParts[i + 2];
            //事件选项功能数据
            string optionData = eventParts[i + 4];
            //事件选项后续
            string optionResult = eventParts[i + 6];
            //创建一个EventBase
            EventBase eventBase = new EventBase(optionDescription, optionResult,optionFunc,optionData);


            
            //将eventBase加入eventList
            eventList.Add(eventBase);
        }
    }
    //改变主角生命值
    public void ChangeHealth(string healNum,string eventResult)
    {
        //确定改变的生命值
        Debug.Log("ChangeHealth");
        Debug.Log(healNum);
        int heal = int.Parse(healNum);
        SaveManager.instance.jsonData.playerData.HP += heal;
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
    }

    //改变主角金币
    public void ChangeCoin(string coinNum, string eventResult)
    {
        //确定改变的金币
        Debug.Log("ChangeCoin");
        int coin = int.Parse(coinNum);
        SaveManager.instance.jsonData.playerData.coin += coin;
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
    }
    //改变主角指骨
    public void ChangeFB(string FbNum, string eventResult)
    {
        int fingerBone = int.Parse(FbNum);
        SaveManager.instance.jsonData.playerData.fingerBone += fingerBone;
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
    }
    //打开OptionPanel
    public void OpenOptionPanel(string panelType, string eventResult,string cardPool)
    {
        //确定CardsPanel的类型
        Debug.Log("OpenOptionPanel");
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
        Instantiate(Resources.Load("Prefabs/UI/OptionPanel"), GameObject.Find("Canvas").transform);
        
        //FindObjectOfType<OptionPanel>().cardPool = (TextAsset)Resources.Load("TextAssets/CardPool/Common");
        //Debug.Log(cardPoolTextAsset.text);
        //FindObjectOfType<OptionPanel>().cardPoolText = cardPoolTextAsset.text;
        if (panelType == "delete")
        {
            FindObjectOfType<OptionPanel>().type = OptionPanel.panelType.delete;
        }
        else if (panelType == "add")
        {
            FindObjectOfType<OptionPanel>().type = OptionPanel.panelType.add;
        }
        //给该OptionPanel传入卡池数据
        FindObjectOfType<OptionPanel>().cardPool = Resources.Load<TextAsset>("TextAssets/CardPool/" + cardPool);
        Debug.Log(Resources.Load<TextAsset>("TextAssets/CardPool/" + cardPool).text);
        OptionPanel.instance.LoadPanel();
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        
    }
    //打开CardsPanel
    public void OpenCardsPanel(string panelType, string eventResult)
    {
        //确定CardsPanel的类型
        Debug.Log("OpenCardsPanel");
        Instantiate(Resources.Load("Prefabs/UI/CardListPanel"),GameObject.Find("Canvas").transform);
        if (panelType == "delete")
        {
            GameObject.Find("AllCardsPanel").GetComponent<DeckBoard>().type = DeckBoard.boardType.delete;
        }
        else if(panelType == "copy")
        {
            GameObject.Find("AllCardsPanel").GetComponent<DeckBoard>().type = DeckBoard.boardType.copy;
        }
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
    }
    //添加指定卡牌
    public void CardAppend(string cardsData,string eventResult)
    {
        string[] cards = cardsData.Split(',');
        foreach(var card in cards)
        {
            SaveManager.instance.jsonData.playerData.playerDeck.Add(card);
        }
        //更改事件描述为事件后续
        eventDescription.text = eventResult;
        //清空选项面板，替换为继续按钮
        ClearOptionContinue();
    }
    public void Countinue()
    {
        Debug.Log("Continue");
        //保存，回到地图
        SaveManager.instance.Save();
    }
    private void ClearOptionContinue()
    {
        for (int i = 0; i < GameObject.Find("choicePanel").transform.childCount; i++)
        {
            Destroy(GameObject.Find("choicePanel").transform.GetChild(i).gameObject);
        }
        if(!GameObject.Find("ContinueButton(Clone)"))
        {
            GameObject continueButton = Instantiate(Resources.Load("Prefabs/UI/ContinueButton"), GameObject.Find("Canvas").transform) as GameObject;
            continueButton.GetComponent<Button>().onClick.AddListener(Countinue);
        }
    }
}
