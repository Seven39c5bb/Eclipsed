using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class FightInit : FightUnit
{
    public TextAsset fightNodeInfo;//储存战斗类节点关卡信息的Txt文件
    private Dictionary<string, List<Vector2Int>> enemyPositions = new Dictionary<string, List<Vector2Int>>();//储存敌人与位置的字典
    private Vector2Int playerInitPos;//玩家初始位置
    private string chessboardSettingName;//棋盘设置文件名
    private CanvasGroup blackCanvas;//黑幕
    public override void Init()
    {
        Debug.Log("this init fightunit init");

        //从日志文件中读取储存该场地信息的txt文件所在路径，并加载该txt文件
        //chessboardSettingName = "chessTestTxt";//记得修改为从日志文件中读取文件名!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        chessboardSettingName = SaveManager.instance.jsonData.mapData.currBattleNodeInfoName;
        fightNodeInfo = Resources.Load<TextAsset>("TextAssets/ChessboardSetting/" + chessboardSettingName);

        //初始化棋盘
        //从文件中读取玩家位置，敌人种类，每个种类的敌人数量，每个敌人的确切位置
        if (fightNodeInfo != null)
        {
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
        }

        //从Assets/Resources/Prefabs/Chesses/Player.prefab处获取玩家预制体
        GameObject player = Resources.Load("Prefabs/Chesses/Player") as GameObject;
        //实例化玩家
        player.GetComponent<PlayerController>().Location = playerInitPos;
        GameObject playerObj = ChessboardManager.Instantiate(player, new Vector3(0, 0, 0), player.transform.rotation);

        //从Assets/Resources/Prefabs/Chesses/敌人种类名.prefab处获取敌人预制体
        foreach (var enemyType in enemyPositions)
        {
            GameObject enemy = Resources.Load("Prefabs/Chesses/" + enemyType.Key) as GameObject;
            //实例化敌人
            foreach (var pos in enemyType.Value)
            {
                enemy.GetComponent<EnemyBase>().Location = pos;
                GameObject enemyObj = ChessboardManager.Instantiate(enemy, new Vector3(0, 0, 0), enemy.transform.rotation);
            }
        }

        //让ChessboardManager更新敌人控制器列表
        ChessboardManager.instance.UpdateEnemyControllerList();

        //开始检测胜利条件
        FightManager.instance.isCheckingVictory = true;

        FightUI.instance.InitEnemyStateBoard();

        //使用Dotween黑幕淡出
        /* blackCanvas = GameObject.Find("BlackCanvas").GetComponent<CanvasGroup>();
        blackCanvas.DOFade(0, 0.5f).OnComplete(() =>
        {
            //初始化完成后切换到玩家回合
            //FightManager.instance.ChangeType(FightType.Player);
        }); */
        
        

        //test:添加几张牌进牌组
        //GameObject obj= Resources.Load("Prefabs/Card/up") as GameObject;
        //CardManager.instance.cardDesk.Add(obj.GetComponent<Card>());
        //初始化完成后切换到玩家回合
        FightManager.instance.ChangeType(FightType.Player);
    }
    public override void OnUpdate()
    {
        
    }
}
