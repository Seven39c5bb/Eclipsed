using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager instance_private;
    public static MapManager instance
    {
        get
        {
            if (instance_private == null)
            {
                instance_private = FindObjectOfType<MapManager>();
                // 如果找不到MapManager实例，那么返回null
                if (instance_private == null)
                {
                    return null;
                }
                instance_private.mapNodes = new MapNode[14][];
                for (int i = 0; i < instance_private.mapNodes.Length; i++)
                {
                    instance_private.mapNodes[i] = new MapNode[3];
                }
            }
            return instance_private;
        }
    }

    public MapNode[][] mapNodes;
    private TextMeshProUGUI HeartText;
    private TextMeshProUGUI CardText;
    private TextMeshProUGUI MoneyText;
    private TextMeshProUGUI BoneText;
    public bool isHunted;
    //public bool MapBeCreated = false;//是否已经创建地图信息
    public bool CreateDone = false;//是否已经生成地图场景
    //public bool isBackFromBattle = false;//是否从战斗场景返回
    public enum AtlasID
    {
        Atlas_1,
        Atlas_2,
        Atlas_3,
        Atlas_4
    }

    public AtlasID currAtlasID;


    private void Awake()
    {
        /* mapNodes = new MapNode[14][];
        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = new MapNode[3];
        } */

        // 从存档中读取当前地图是否被创建(在进入新地图时，记得将其置为false)
        // MapBeCreated = SaveManager.instance.jsonData.mapData.mapBeCreated;

        AudioManager.PlayBGM(currAtlasID.ToString());//播放当前地图的背景音乐
        currAtlasID = SaveManager.instance.jsonData.mapData.currAtlasID;
    }

    private void Start()
    {
        // 节点将在自己的start中被传入mapNodes，因此不要在这里使用mapNodes，否则会出现空指针异常

        // 从场景中找到名为HeartText，CardText，MoneyText，BoneText的TMP组件
        HeartText = GameObject.Find("Canvas/InfoBoard/HeartBackground/HeartText").GetComponent<TextMeshProUGUI>();
        CardText = GameObject.Find("Canvas/InfoBoard/CardBackground/CardText").GetComponent<TextMeshProUGUI>();
        MoneyText = GameObject.Find("Canvas/InfoBoard/MoneyBackground/MoneyText").GetComponent<TextMeshProUGUI>();
        BoneText = GameObject.Find("Canvas/InfoBoard/BoneBackground/BoneText").GetComponent<TextMeshProUGUI>();

        if (HeartText == null || CardText == null || MoneyText == null || BoneText == null)
        {
            Debug.LogError("Can't find TextMeshPro component");
        }
        // 从存档中读取玩家数据
        HeartText.text = SaveManager.instance.jsonData.playerData.HP.ToString() + "/" + SaveManager.instance.jsonData.playerData.MaxHP.ToString();
        CardText.text = SaveManager.instance.jsonData.playerData.playerDeck.Count.ToString();
        MoneyText.text = SaveManager.instance.jsonData.playerData.coin.ToString();
        BoneText.text = SaveManager.instance.jsonData.playerData.fingerBone.ToString();
    }

    void Update()
    {
        if (!CreateDone)//初始化地图
        {
            CreateDone = true;

            if (!SaveManager.instance.jsonData.mapData.mapBeCreated)
            {
                //MapBeCreated = true;
                SaveManager.instance.jsonData.mapData.mapBeCreated = true;


                // 生成地图
                // (0,1)为起点，其后继节点为(1,0),(1,1),(1,2)
                mapNodes[0][1].nextNodes.Add(mapNodes[1][0]);
                mapNodes[0][1].nextNodes.Add(mapNodes[1][1]);
                mapNodes[0][1].nextNodes.Add(mapNodes[1][2]);

                // (4,1)为中间节点，其后继节点为(5,0),(5,1),(5,2)；(3,0),(3,1),(3,2)的后继节点为(4,1)
                // (8,1)为中间节点，其后继节点为(9,0),(9,1),(9,2)；(7,0),(7,1),(7,2)的后继节点为(8,1)
                for (int i = 4; i < 10; i += 4)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mapNodes[i][1].nextNodes.Add(mapNodes[i + 1][j]);
                        mapNodes[i - 1][j].nextNodes.Add(mapNodes[i][1]);
                    }
                }

                // (12,1)为前商店，其前驱节点为(11,0),(11,1),(11,2)
                mapNodes[11][0].nextNodes.Add(mapNodes[12][1]);
                mapNodes[11][1].nextNodes.Add(mapNodes[12][1]);
                mapNodes[11][2].nextNodes.Add(mapNodes[12][1]);

                // (13,1)为终点，其前驱节点为(12,1)
                mapNodes[12][1].nextNodes.Add(mapNodes[13][1]);

                // 除已确定后继的节点，其他节点的后继节点为下一层的同行节点
                for (int i = 1; i < 11; i++)
                {
                    if (i == 3 || i == 4 || i == 7 || i == 8)
                        continue;
                    for (int j = 0; j < 3; j++)
                    {
                        mapNodes[i][j].nextNodes.Add(mapNodes[i + 1][j]);
                    }
                }

                // 所有节点的左右节点为同层相邻的节点（若有的话）
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (mapNodes[i][j] == null)
                            continue;
                        if (j > 0)
                            if (mapNodes[i][j - 1] != null)
                                mapNodes[i][j].leftNode = mapNodes[i][j - 1];
                        if (j < 2)
                            if (mapNodes[i][j + 1] != null)
                                mapNodes[i][j].rightNode = mapNodes[i][j + 1];
                    }
                }


                // 两个中间节点分出了三个区域，分别为(1,0)-(3,2)、(5,0)-(7,2)、(9,0)-(11,2)，每个区域内的节点都只有行是相通的
                // 现在需要在每个区域新增0 - 4条岔路，使得每条行路径可以通过岔路到达其他行
                // 定义每个区域的范围
                int[][] regions = new int[][] {
                    new int[] {1, 3},
                    new int[] {5, 7},
                    new int[] {9, 11}
                };

                // 对每个区域进行操作
                foreach (int[] region in regions)
                {
                    // 随机生成岔路的数量
                    int numBranches = Random.Range(0, 5);

                    for (int i = 0; i < numBranches; i++)
                    {
                        MapNode startNode, endNode, oppositeStartNode, oppositeEndNode;
                        do
                        {
                            // 随机选择一个起始节点和一个结束节点
                            int startRow = Random.Range(region[0], region[1]);
                            int endRow = startRow + 1;
                            int startCol = Random.Range(0, 3);
                            int direction = Random.Range(0, 2)*2 - 1;
                            int endCol = startCol + direction;
                            if (endCol < 0 || endCol > 2)
                                endCol = startCol - direction;

                            startNode = mapNodes[startRow][startCol];
                            oppositeStartNode = mapNodes[startRow][endCol];
                            endNode = mapNodes[endRow][endCol];
                            oppositeEndNode = mapNodes[endRow][startCol];
                        } while (startNode.nextNodes.Contains(endNode) || oppositeStartNode.nextNodes.Contains(oppositeEndNode)); // 如果岔路已经存在，那么重新选择

                        // 生成岔路
                        startNode.nextNodes.Add(endNode);
                    }

                }

                // 针对地区一二三，有不同的节点类型分配方案
                // 区域1为 (1,0)-(3,2)
                // 区域2为 (5,0)-(7,2)
                // 区域3为 (9,0)-(11,2)
                List<MapNode.NodeType> nodeTypes = new List<MapNode.NodeType>();
                switch (currAtlasID)
                {
                    case AtlasID.Atlas_1:
                        // 为每个节点分配类型
                        // 每个region有不同的分配方案
                        // region 1
                        // Hunting * 1 , Event * (1 ~ 3) , 剩下的是Fight;
                        // 创建一个列表，其中包含所有的节点类型

                        // 添加Hunting节点
                        nodeTypes.Add(MapNode.NodeType.Hunting);

                        // 随机添加1到3个Event节点
                        int numEvents = Random.Range(1, 4);
                        for (int i = 0; i < numEvents; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }

                        // 剩下的都是Fight节点
                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }

                        // 为该区域每个节点随机分配一个类型
                        for (int row = 1; row <= 3; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 1) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 1, Random.Range(1, 6));//最大文件id为5
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 1, Random.Range(1, 6));//最大文件id为5
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }
                        
                        // region 2
                        // Hunting * 1 , Elite * (1 ~ 2) , Event * (1 ~ 2) , 剩下的是Fight;
                        nodeTypes.Clear();
                        nodeTypes.Add(MapNode.NodeType.Hunting);
                        int numElites = Random.Range(1, 3);
                        for (int i = 0; i < numElites; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Elite);
                        }
                        int numEvents2 = Random.Range(1, 3);
                        for (int i = 0; i < numEvents2; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }
                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }
                        for (int row = 5; row <= 7; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 5) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 4));//最大文件id为3
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 4));//最大文件id为3
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    if (node.nodeType == MapNode.NodeType.Elite)
                                    {
                                        node.InfoFileName = GnerateEliteNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 3));//最大文件id为2
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }

                        // region 3
                        // Hunting * 1 , Elite * 2 , Event * (1 ~ 2) , 剩下的是Fight;
                        nodeTypes.Clear();
                        nodeTypes.Add(MapNode.NodeType.Hunting);
                        for (int i = 0; i < 2; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Elite);
                        }
                        int numEvents3 = Random.Range(1, 3);
                        for (int i = 0; i < numEvents3; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }
                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }
                        for (int row = 9; row <= 11; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 9) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    if (node.nodeType == MapNode.NodeType.Elite)
                                    {
                                        node.InfoFileName = GnerateEliteNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }

                        //(0,1)和(8,1)为Plot节点,(4,1)和(12,1)为Shop节点,(13,1)为Boss节点
                        mapNodes[0][1].nodeType = MapNode.NodeType.Plot;
                        mapNodes[4][1].nodeType = MapNode.NodeType.Shop;
                        mapNodes[8][1].nodeType = MapNode.NodeType.Plot;
                        mapNodes[12][1].nodeType = MapNode.NodeType.Shop;
                        mapNodes[13][1].nodeType = MapNode.NodeType.Boss;
                        mapNodes[13][1].InfoFileName = GnerateBossNodeInfoName(AtlasIDToInt(currAtlasID), 1);//第一地区boss信息id为1

                        //遍历所有事件节点，为其分配事件信息
                        List<int> usedEventIds = new List<int>();
                        for (int i = 1; i < 13; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (mapNodes[i][j] == null)
                                    continue;
                                if (mapNodes[i][j].nodeType == MapNode.NodeType.Event)
                                {
                                    int eventId;
                                    do
                                    {
                                        eventId = Random.Range(1, 9);
                                    } while (usedEventIds.Contains(eventId));

                                    usedEventIds.Add(eventId);
                                    mapNodes[i][j].InfoFileName = GenerateEventNodeInfoName(AtlasIDToInt(currAtlasID), eventId);
                                }
                            }
                        }
                        break;

                    case AtlasID.Atlas_2:
                    case AtlasID.Atlas_3:
                        // 为每个节点分配类型
                        // 每个region有不同的分配方案
                        // 若isHunted，则Hunting节点变为Elite节点

                        // region 1
                        // Hunting * 1 , Elite * 1 , Event * (1 ~ 3) , 剩下的是Fight;
                        if (isHunted) nodeTypes.Add(MapNode.NodeType.Elite);
                        else nodeTypes.Add(MapNode.NodeType.Hunting);

                        nodeTypes.Add(MapNode.NodeType.Elite);

                        int numEvents4 = Random.Range(1, 4);
                        for (int i = 0; i < numEvents4; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }

                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }

                        for (int row = 1; row <= 3; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 1) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 1, Random.Range(1, 4));//最大文件id为3
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 1, Random.Range(1, 4));//最大文件id为3
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    if (node.nodeType == MapNode.NodeType.Elite)
                                    {
                                        node.InfoFileName = GnerateEliteNodeInfoName(AtlasIDToInt(currAtlasID), 1, Random.Range(1, 3));//最大文件id为2
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }

                        // region 2
                        // Hunting * 1 , Elite * (1 ~ 2) , Event * (1 ~ 2) , 剩下的是Fight;
                        nodeTypes.Clear();
                        if (isHunted) nodeTypes.Add(MapNode.NodeType.Elite);
                        else nodeTypes.Add(MapNode.NodeType.Hunting);

                        int numElites2 = Random.Range(1, 3);

                        for (int i = 0; i < numElites2; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Elite);
                        }

                        int numEvents5 = Random.Range(1, 3);
                        for (int i = 0; i < numEvents5; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }

                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }

                        for (int row = 5; row <= 7; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 5) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 4));//最大文件id为3
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 4));//最大文件id为3
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    if (node.nodeType == MapNode.NodeType.Elite)
                                    {
                                        node.InfoFileName = GnerateEliteNodeInfoName(AtlasIDToInt(currAtlasID), 2, Random.Range(1, 3));//最大文件id为2
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }

                        // region 3
                        // Hunting * 1 , Elite * 2 , Event * (1 ~ 2) , 剩下的是Fight;
                        nodeTypes.Clear();
                        if (isHunted) nodeTypes.Add(MapNode.NodeType.Elite);
                        else nodeTypes.Add(MapNode.NodeType.Hunting);

                        for (int i = 0; i < 2; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Elite);
                        }

                        int numEvents6 = Random.Range(1, 3);
                        for (int i = 0; i < numEvents6; i++)
                        {
                            nodeTypes.Add(MapNode.NodeType.Event);
                        }

                        while (nodeTypes.Count < 7)
                        {
                            nodeTypes.Add(MapNode.NodeType.Fight);
                        }

                        for (int row = 9; row <= 11; row++)
                        {
                            for (int col = 0; col <= 2; col++)
                            {
                                MapNode node = mapNodes[row][col];
                                if (row == 9) // 如果是第一层，设置为战斗
                                {
                                    node.nodeType = MapNode.NodeType.Fight;
                                    node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                }
                                else
                                {
                                    int index = Random.Range(0, nodeTypes.Count);
                                    node.nodeType = nodeTypes[index];
                                    if (node.nodeType == MapNode.NodeType.Fight)
                                    {
                                        node.InfoFileName = GenerateFightNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                    }
                                    if (node.nodeType == MapNode.NodeType.Hunting)
                                    {
                                        node.InfoFileName = GnerateHuntingNodeInfoName(AtlasIDToInt(currAtlasID));
                                    }
                                    if (node.nodeType == MapNode.NodeType.Elite)
                                    {
                                        node.InfoFileName = GnerateEliteNodeInfoName(AtlasIDToInt(currAtlasID), 3, Random.Range(1, 3));//最大文件id为2
                                    }
                                    nodeTypes.RemoveAt(index);
                                }
                            }
                        }

                        //(0,1)和(8,1)为Plot节点,(4,1)和(12,1)为Shop节点,(13,1)为Boss节点
                        mapNodes[0][1].nodeType = MapNode.NodeType.Plot;
                        mapNodes[4][1].nodeType = MapNode.NodeType.Shop;
                        mapNodes[8][1].nodeType = MapNode.NodeType.Plot;
                        mapNodes[12][1].nodeType = MapNode.NodeType.Shop;
                        mapNodes[13][1].nodeType = MapNode.NodeType.Boss;
                        mapNodes[13][1].InfoFileName = GnerateBossNodeInfoName(AtlasIDToInt(currAtlasID), 1);//地区boss信息id为1

                        //遍历所有事件节点，为其分配事件信息
                        List<int> usedEventIds2 = new List<int>();
                        for (int i = 1; i < 13; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (mapNodes[i][j] == null)
                                    continue;
                                if (mapNodes[i][j].nodeType == MapNode.NodeType.Event)
                                {
                                    int eventId;
                                    do
                                    {
                                        eventId = Random.Range(1, 9);
                                    } while (usedEventIds2.Contains(eventId));

                                    usedEventIds2.Add(eventId);
                                    mapNodes[i][j].InfoFileName = GenerateEventNodeInfoName(AtlasIDToInt(currAtlasID), eventId);
                                }
                            }
                        }
                        break;
                }


                // 遍历所有节点，根据后继节点生成路径
                for (int i = 0; i <= 13; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (mapNodes[i][j] == null)
                            continue;
                        mapNodes[i][j].PathGenerate();

                        // 根据节点类型分配精灵
                        mapNodes[i][j].SetNodeSprite();

                    }
                }

                //保存一次
                SaveManager.instance.Save();
            }
            else
            {
                SaveManager.instance.Load();
            }

            if (SaveManager.instance.isBackFromNodeScene)
            {
                // 从战斗场景返回时，将刚刚的节点设置为已探索
                Debug.Log("Back from node scene");
                SaveManager.instance.isBackFromNodeScene = false;//仅当从节点场景中返回时，才为true，将节点设置为已访问
                mapNodes[SaveManager.instance.jsonData.mapData.currNodeID.x][SaveManager.instance.jsonData.mapData.currNodeID.y].VisitNode();
            }

            // 遍历所有节点
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (mapNodes[i][j] == null)
                        continue;
                    if (mapNodes[i][j].nodeType != MapNode.NodeType.Boss)//不是boss节点，则将节点的返回地图id设置为当前地图id
                    {
                        mapNodes[i][j].backAtlasID = currAtlasID;
                    }
                }
            }

            SaveManager.instance.Save();
        }
    }


    public static int AtlasIDToInt(AtlasID id)
    {
        switch (id)
        {
            case AtlasID.Atlas_1:
                return 1;
            case AtlasID.Atlas_2:
                return 2;
            case AtlasID.Atlas_3:
                return 3;
            case AtlasID.Atlas_4:
                return 4;
            default:
                return 0;
        }
    }
    public static string AtlasIDToString(AtlasID id)
    {
        switch (id)
        {
            case AtlasID.Atlas_1:
                return "Atlas_1";
            case AtlasID.Atlas_2:
                return "Atlas_2";
            case AtlasID.Atlas_3:
                return "Atlas_3";
            case AtlasID.Atlas_4:
                return "Atlas_4";
            default:
                return "";
        }
    }

    public string GenerateFightNodeInfoName(int atlasNumber, int regionNumber, int infoId)
    {
        return string.Format("Fight_{0}_{1}_{2}", atlasNumber, regionNumber, infoId);
    }

    public string GnerateHuntingNodeInfoName(int atlasNumber)
    {
        return string.Format("Hunting_{0}", atlasNumber);
    }

    public string GnerateEliteNodeInfoName(int atlasNumber, int regionNumber, int infoId)
    {
        return string.Format("Elite_{0}_{1}_{2}", atlasNumber, regionNumber, infoId);
    }

    public string GnerateBossNodeInfoName(int atlasNumber, int infoId)
    {
        return string.Format("BOSS_{0}_{1}", atlasNumber, infoId);
    }

    public string GenerateEventNodeInfoName(int atlasNumber, int infoId)
    {
        return string.Format("Event_{0}_{1}", atlasNumber, infoId);
    }
}
