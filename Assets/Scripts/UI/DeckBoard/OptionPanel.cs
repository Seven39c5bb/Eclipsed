using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    public enum panelType
    {
        delete,
        add
    }
    public List<CardTemplate> cardTemplates;
    public panelType type;
    //从卡池TextAsset中读取卡牌信息
    public TextAsset cardPool;
    private void Awake()
    {
        LoadPanel();
    }
    public void LoadPanel()
    {
        //
        string[] cards = cardPool.text.Split(',');
        //从牌池中随机挑选3张卡牌
        //加载3张卡牌模板挂在该物体上
        for (int i = 0; i < 3; i++)
        {
            GameObject card = Instantiate(Resources.Load("Prefabs/UI/CardTemplate"), transform) as GameObject;
            CardTemplate cardTemplate = card.GetComponent<CardTemplate>();
            cardTemplates.Add(cardTemplate);
            GameObject cardMes = Resources.Load<GameObject>("Prefabs/Card/" + cards[Random.Range(0,cards.Length)]);
            //获取cardMes上信息
            if (cardMes != null)
            {
                cardTemplate.card_Name = cardMes.name;
                //Debug.Log(cardMes.transform.GetChild(0).name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
            }
        }
        
    }
}
