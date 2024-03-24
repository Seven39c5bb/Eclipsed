using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameConfig
{
    public TextAsset deckAsset;
    public Dictionary<string, int> cardDeckData;
    
    public void Init()
    {
        cardDeckData = new Dictionary<string, int>();
        //获取卡组文件txt 
        deckAsset = Resources.Load<TextAsset>("TextAssets/Initial deck");
        //Debug.Log(deckAsset.text);
        string[] lines = deckAsset.text.Split('\n');
        
        for (int i = 1; i < lines.Length; i++)
        {
            string[] word = lines[i].Split("*");
            int flag = 1;string cardName="";int cardCount=0;
            foreach (string word2 in word)
            {
                if (flag == 1)
                {
                    cardName = word2;
                    flag++;
                }
                else
                {
                    cardCount = int.Parse(word2);
                }
            }
            //Debug.Log(cardName+cardCount);
            //将卡牌名字和卡牌数量加进carddeckdata中
            cardDeckData.Add(cardName,cardCount);
        }
        //Debug.Log(cardDeckData);
    }
}
