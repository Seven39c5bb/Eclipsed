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
                //如果没有任何功能
                else
                {
                    string eventResult = eventList[i].optionResult;
                    buttonObj.GetComponent<Button>().onClick.AddListener(() =>  Leave(eventResult));
                }
                #endregion
            }
        }
    }

    //处理eventAsset
    public void HandleEventAsset()
    {
        //从eventAsset中读取事件信息
        eventAsset = Resources.Load<TextAsset>("TextAssets/events/" + SaveManager.instance.jsonData.mapData.currNodeInfoName);
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
            //Debug.Log(optionDescription);
            //事件选项功能
            string optionFunc = eventParts[i + 2];
            //Debug.Log(optionFunc);
            //事件选项功能数据
            string optionData = eventParts[i + 4];
            //Debug.Log(optionData);
            //事件选项后续
            string optionResult = eventParts[i + 6];
            //Debug.Log(optionResult);
            //创建一个EventBase
            EventBase eventBase = new EventBase(optionDescription, optionResult,optionFunc,optionData);


            
            //将eventBase加入eventList
            eventList.Add(eventBase);
        }
    }
    //不带任何效果
    public void Leave(string eventResult)
    {
        eventDescription.text = eventResult; 
        ClearOptionContinue();
    }
    //改变主角生命值
    public void ChangeHealth(string healNum,string eventResult)
    {
        //确定改变的生命值
        Debug.Log("ChangeHealth");
        Debug.Log(healNum);
        int heal = int.Parse(healNum);
        SaveManager.instance.jsonData.playerData.HP += heal;
        if (SaveManager.instance.jsonData.playerData.HP > SaveManager.instance.jsonData.playerData.MaxHP)//确保生命值不超过最大生命值
        {
            SaveManager.instance.jsonData.playerData.HP = SaveManager.instance.jsonData.playerData.MaxHP;
        }
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
        if (cardPool == "Default")
        {
            string cardTxt = "";
            foreach(string card in SaveManager.instance.jsonData.playerData.playerDeck)
            {
                cardTxt += card + ",";
            }
            FindObjectOfType<OptionPanel>().cardPoolText = cardTxt;
        }
        else
        {
            FindObjectOfType<OptionPanel>().cardPool = Resources.Load<TextAsset>("TextAssets/CardPool/" + cardPool);
        }
        //Debug.Log(Resources.Load<TextAsset>("TextAssets/CardPool/" + cardPool).text);
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
        //根据当前地图ID返回对应地图
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
