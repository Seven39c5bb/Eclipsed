using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public enum panelType
    {
        delete,
        add,
        win
    }
    public List<CardTemplate> cardTemplates;
    public panelType type;
    //从卡池TextAsset中读取卡牌信息
    public TextAsset cardPool;
    public string cardPoolText;
    public static OptionPanel op_instance;
    public static OptionPanel instance
    {
        get
        {
            if (op_instance == null)
            {
                op_instance = FindObjectOfType<OptionPanel>();
            }
            return op_instance;
        }
    }
    public void LoadPanel()
    {
        
        if(cardPool==null&& cardPoolText == "")
        {
            Debug.LogError("卡池为空");
            return;
        }
        if (cardPool != null)
        {
            cardPoolText = cardPool.text;
        }
        

        string[] cards = cardPoolText.Split(',');
        
        List<int> usedIndexes = new List<int>(); // 用于存储已使用的索引
        //从牌池中随机挑选3张卡牌
        //加载3张卡牌模板挂在该物体上
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, cards.Length); // 随机生成索引
            } while (usedIndexes.Contains(randomIndex)); // 如果已使用的索引中包含该索引，则重新生成
            usedIndexes.Add(randomIndex); // 将索引添加到已使用列表中

            GameObject card = Instantiate(Resources.Load("Prefabs/UI/CardTemplate"), transform) as GameObject;
            CardTemplate cardTemplate = card.GetComponent<CardTemplate>();
            cardTemplates.Add(cardTemplate);
            GameObject cardMes = Resources.Load<GameObject>("Prefabs/Card/" + cards[randomIndex]);
            //获取cardMes上信息
            if (cardMes != null)
            {
                cardTemplate.card_Name = cardMes.name;
                //Debug.Log(cardMes.transform.GetChild(0).name);
                cardTemplate.cardName.text = cardMes.GetComponent<Card>().cardName;
                cardTemplate.cardDescription.text = cardMes.GetComponent<Card>().discription;
                cardTemplate.cardCost.text = cardMes.GetComponent<Card>().cost.ToString();
                switch (cardMes.GetComponent<Card>().rare)
                {
                    case Card.rareType.common:
                        cardTemplate.cardName.color = Color.white;
                        break;
                    case Card.rareType.rare:
                        cardTemplate.cardName.color = Color.blue;
                        break;
                    case Card.rareType.legend:
                        cardTemplate.cardName.color = new Color(1, (float)(148 / 255), (float)(68 / 255));
                        break;
                }
                Sprite[] sprites = Resources.LoadAll<Sprite>("Pictures/CardImg/cardTypeImg");
                switch (cardMes.GetComponent<Card>().type)
                {
                    case Card.cardType.action:
                        cardTemplate.cardTypeImg.sprite = sprites[0];
                        break;
                    case Card.cardType.skill:
                        cardTemplate.cardTypeImg.sprite = sprites[4];
                        break;
                    case Card.cardType.rites:
                        cardTemplate.cardTypeImg.sprite = sprites[3];
                        break;
                    case Card.cardType.trap:
                        cardTemplate.cardTypeImg.sprite = sprites[5];
                        break;
                }
                //更换卡面
                if (Resources.Load("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName) != null)
                {
                    cardTemplate.GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/CardImg/cardTmpImg/" + cardMes.GetComponent<Card>().cardName);
                }
            }
        }

    }
}
