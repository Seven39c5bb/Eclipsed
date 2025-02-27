using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    //获取该面板上的3个物体，如果点击该物体则触发相应事件
    public GameObject cardAppendBtm;
    public GameObject HealBtm;
    public GameObject coinBtm;
    public TMPro.TextMeshProUGUI tipsText;
    public GameObject winContinueBtm;

    //如果点击了cardAppend物体，触发CardAppend事件
    public void CardAppend()
    {
        //如果点击了卡牌增加按钮,弹出OptionPanel
        GameObject optionPanel = Instantiate(Resources.Load("Prefabs/UI/OptionPanelBox"), GameObject.Find("Canvas").transform) as GameObject;
        OptionPanel.instance.type = OptionPanel.panelType.add;
        if(SaveManager.instance.jsonData.mapData.currNodeType==MapNode.NodeType.Elite)
        {
            OptionPanel.instance.cardPool = Resources.Load("TextAssets/CardPool/All") as TextAsset;
        }
        else if(SaveManager.instance.jsonData.mapData.currNodeType == MapNode.NodeType.Fight)
        {
            OptionPanel.instance.cardPool = Resources.Load("TextAssets/CardPool/Fight") as TextAsset;
        }
        //else if (SaveManager.instance.jsonData.mapData.currNodeType == MapNode.NodeType.Boss)
        //{
        //    OptionPanel.instance.cardPool = Resources.Load("TextAssets/CardPool/Boss") as TextAsset;
        //}
        OptionPanel.instance.LoadPanel();
        Debug.Log("卡牌增加");
        SaveManager.instance.jsonData.playerData.HP = PlayerController.instance.HP;
        SaveManager.instance.jsonData.playerData.coin = PlayerController.instance.coins;
        if (Lupin.instance != null)
        {
            SaveManager.instance.jsonData.lupinData.HP = Lupin.instance.HP;
            SaveManager.instance.jsonData.lupinData.coin = Lupin.instance.lupinWallet;
        }
        if (SaveManager.instance.jsonData.playerData.HP > SaveManager.instance.jsonData.playerData.MaxHP)//确保生命值不超过最大生命值
        {
            SaveManager.instance.jsonData.playerData.HP = SaveManager.instance.jsonData.playerData.MaxHP;
        }
        int coin = Random.Range(20, 30);
        SaveManager.instance.jsonData.playerData.coin += coin;
        tipsText.text = "*新增了卡牌,获得了*"+coin.ToString()+"金币";
        winContinueBtm.SetActive(true);
        Destroy(HealBtm); Destroy(coinBtm); Destroy(cardAppendBtm);
    }
    //如果点击了Heal物体，触发Heal事件
    public void Heal()
    {
        //如果点击了治疗按钮，则治疗
        Debug.Log("治疗");
        SaveManager.instance.jsonData.playerData.HP = PlayerController.instance.HP;
        SaveManager.instance.jsonData.playerData.HP += 20;
        if (SaveManager.instance.jsonData.playerData.HP > SaveManager.instance.jsonData.playerData.MaxHP)//确保生命值不超过最大生命值
        {
            SaveManager.instance.jsonData.playerData.HP = SaveManager.instance.jsonData.playerData.MaxHP;
        }
        int coin = Random.Range(20, 30);
        SaveManager.instance.jsonData.playerData.coin += coin;
        tipsText.text = "*生命+20,*获得了"+coin.ToString()+"金币";
        winContinueBtm.SetActive(true);
        SaveManager.instance.Save();
        Destroy(HealBtm);Destroy(coinBtm);Destroy(cardAppendBtm);
    }
    //如果点击了coin物体，触发Coin事件
    public void Coin()
    {
        //如果点击了金币按钮，则增加金币
        Debug.Log("金币");
        int coin = Random.Range(20, 30);
        SaveManager.instance.jsonData.playerData.coin += coin;
        SaveManager.instance.jsonData.playerData.fingerBone += 1;
        SaveManager.instance.jsonData.playerData.HP = PlayerController.instance.HP;
        if (SaveManager.instance.jsonData.playerData.HP > SaveManager.instance.jsonData.playerData.MaxHP)//确保生命值不超过最大生命值
        {
            SaveManager.instance.jsonData.playerData.HP = SaveManager.instance.jsonData.playerData.MaxHP;
        }
        tipsText.text = "*金币+"+coin.ToString()+" 指骨+1*";
        winContinueBtm.SetActive(true);
        Destroy(HealBtm); Destroy(coinBtm); Destroy(cardAppendBtm);
    }
}
