using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class Test : MonoBehaviour
{
    //public SpriteRenderer Renderer;
    public TextAsset fightNodeInfo;//储存战斗类节点关卡信息的Txt文件
    private Dictionary<string, List<Vector2Int>> enemyPositions = new Dictionary<string, List<Vector2Int>>();//储存敌人与位置的字典
    private Vector2Int playerInitPos;//玩家初始位置
    private void Start()
    {
        fightNodeInfo = Resources.Load<TextAsset>("TextAssets/ChessboardSetting/chessTestTxt");

        using (StringReader reader = new StringReader(fightNodeInfo.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key)
                {
                    case "玩家位置":
                        string[] playerXY = value.Split(',');
                        playerInitPos = new Vector2Int(int.Parse(playerXY[0]), int.Parse(playerXY[1]));
                        break;
                    case "敌人种类":
                        string[] enemyTypes = value.Split(',');
                        foreach (string enemyType in enemyTypes)
                        {
                            // 为每种敌人类型初始化一个空列表
                            enemyPositions[enemyType] = new List<Vector2Int>();
                        }
                        break;
                    default:
                        if (enemyPositions.ContainsKey(key))
                        {
                            string[] positions = value.Split(';');
                            foreach (string pos in positions)
                            {
                                string[] xy = pos.Split(',');
                                Vector2Int position = new Vector2Int(int.Parse(xy[0]), int.Parse(xy[1]));
                                enemyPositions[key].Add(position);
                            }
                        }
                        break;
                }
            }
        }

        //通过Debug.Log检查获取的结果是否正确
        Debug.Log("玩家位置：" + playerInitPos);
        foreach (var enemyType in enemyPositions)
        {
            Debug.Log("敌人种类：" + enemyType.Key);
            foreach (var pos in enemyType.Value)
            {
                Debug.Log("敌人位置：" + pos);
            }
        }
    }
    public bool cureButton = false;
    private void Update()
    {
        if(cureButton)
        {
            PlayerController.instance.Cure(20);
            cureButton = false;
        }
    }
    private void OnMouseEnter()
    {
        //Renderer.color = Color.red;
    }
    private void OnMouseExit()
    {
        //Renderer.color = Color.white;
    }

    public void Try()
    {
        Debug.Log("Try");
    }

    public void OnParticleSystemStopped()
    {
        Debug.Log("OnParticleSystemStopped");
    }

}
