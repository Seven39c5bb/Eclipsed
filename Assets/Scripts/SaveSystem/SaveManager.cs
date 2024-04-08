using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class JsonData
{
    public MapData mapData;
    public PlayerData playerData;
}
[System.Serializable]
public class NodesListUnit
{
    public List<Vector2Int> nextNodes;
    public Vector2Int leftNode;
    public Vector2Int rightNode;
    public bool isLocked;
    public Color color;
    public MapNode.NodeType nodeType;
    public Vector2 position;
    public string battleNodeInfoName;
}
[System.Serializable]
public class MapData
{
    public bool mapBeCreated;
    public MapManager.AtlasID currAtlasID;//当前地图ID
    public MapManager.AtlasID backAtlasID;//返回地图ID
    public List<NodesListUnit> mapNodes;
    public string currBattleNodeInfoName;//当前战斗节点信息文件名,用于进入战斗场景后的读取
    public Vector2Int currNodeID;//当前节点ID,用于从其他场景返回时，将该节点设置为已探索，将其子节点解锁
}
[System.Serializable]
public class PlayerData
{
    public int MaxHP = 80;
    public int HP = 80;//当前生命值,在战斗胜利时更新,在进入战斗场景后的读取

    public List<string> playerDeck;//玩家卡组

    public int coin;
    public int fingerBone;//金币
}
public class SaveManager : MonoBehaviour
{
    public bool isBackFromNodeScene;//是否从节点场景返回,在战斗胜利时将之设为true,在加载回Atlas场景之前!!!!!!!!!!!!!!!!!!!!
    public JsonData jsonData;
    private static SaveManager S_instance;
    public static SaveManager instance
    {
        get
        {
            if(S_instance == null)
            {
                S_instance = GameObject.FindObjectOfType<SaveManager>();
            }
            return S_instance;
        }
    }
    private void Awake()
    {
        S_instance = this;
        InitJsonData();
        DontDestroyOnLoad(this);

        Load();
    }
    private void Update()
    {

    }
    //初始化数据
    public void InitJsonData()
    {
        jsonData = new JsonData();
        jsonData.mapData = new MapData();
        jsonData.playerData = new PlayerData();
        jsonData.mapData.mapNodes = new List<NodesListUnit>();
        jsonData.playerData.playerDeck = new List<string>();
        #region 初始化初始卡组
        GameConfig gameConfig = new GameConfig();
        gameConfig.Init();
        foreach (KeyValuePair<string, int> ele in gameConfig.cardDeckData)
        {
            string cardName = ele.Key;
            int cardCount = ele.Value;
            for (int i = 0; i < cardCount; i++)///test 5 �Ļ�(int)cardCount
            {
                jsonData.playerData.playerDeck.Add(cardName);
            }
        }
        #endregion
        //test 初始化金币为100
        jsonData.playerData.coin = 100;
        //test
    }
    //保存数据到json文件
    public void Save()
    {
        UpdateCurDate();
        
        if (!ExistJson())
        {
            Debug.Log("保存文件的所在位置为：" + Application.persistentDataPath);
            File.Create(JsonPath()).Close();
            AssetDatabase.Refresh();
        }
        string json = JsonUtility.ToJson(jsonData, true);
        File.WriteAllText(JsonPath(), json);
        Debug.Log("保存成功");
    }
    //读取数据
    public void Load()
    {
        if (ExistJson())
        {
            string json = File.ReadAllText(JsonPath());
            jsonData = JsonUtility.FromJson<JsonData>(json);
            Debug.Log("读取成功");
            UpdateInfo();
        }
        else
        {
            Debug.Log("读取失败");
        }
    }
    //更新本脚本当前数据
    private void UpdateCurDate()//记得添加数据前记得清空list，防止重复添加！！！！！！！！！
    {
        //如果MapManager.Instance存在
        if (MapManager.Instance != null)
        {
            //jsonData.mapData.mapBeCreated = MapManager.Instance.MapBeCreated;//地图是否被创建

            jsonData.mapData.currAtlasID = MapManager.Instance.currAtlasID;//当前地图ID
            jsonData.mapData.mapNodes.Clear();

            foreach(var nodes in MapManager.Instance.mapNodes)//从当前地图中更新节点数据
            {
                foreach(var node in nodes)
                {
                    if (node != null)
                    {
                        NodesListUnit nodesListUnit = new NodesListUnit();
                        nodesListUnit.nextNodes = new List<Vector2Int>();
                        foreach (var nextNode in node.nextNodes)//后继节点们
                        {
                            nodesListUnit.nextNodes.Add(nextNode.nodeId);
                        }

                        nodesListUnit.isLocked = node.isLocked;//是否被锁定
                        

                        nodesListUnit.color = node.Renderer.color;//颜色

                        if (node.leftNode != null)//左节点
                        {
                            nodesListUnit.leftNode = node.leftNode.nodeId;
                        }
                        else
                        {
                            nodesListUnit.leftNode = new Vector2Int(-1, -1);
                        }

                        if (node.rightNode != null)//右节点
                        {
                            nodesListUnit.rightNode = node.rightNode.nodeId;
                        }
                        else
                        {
                            nodesListUnit.rightNode = new Vector2Int(-1, -1);
                        }

                        nodesListUnit.nodeType = node.nodeType;//节点类型

                        nodesListUnit.position = node.transform.position;//位置

                        nodesListUnit.battleNodeInfoName = node.battleNodeInfoName;//战斗节点信息文件名

                        jsonData.mapData.mapNodes.Add(nodesListUnit);//添加节点数据

                    }
                    else
                    {
                        jsonData.mapData.mapNodes.Add(null);
                    }
                }
            }
        }

    }
    //更新数据
    private void UpdateInfo()
    {
        //如果MapManager.Instance存在
        if (MapManager.Instance != null)
        { 
            //MapManager.Instance.MapBeCreated = jsonData.mapData.mapBeCreated;
            for (int i = 0; i < MapManager.Instance.mapNodes.Length; i++)
            {
                for (int j = 0; j < MapManager.Instance.mapNodes[i].Length; j++)
                {
                    if (MapManager.Instance.mapNodes[i][j] != null)
                    {
                        MapManager.Instance.mapNodes[i][j].isLocked = jsonData.mapData.mapNodes[i * 3 + j].isLocked;
                        MapManager.Instance.mapNodes[i][j].nextNodes = new List<MapNode>();
                        foreach (var nextNode in jsonData.mapData.mapNodes[i * 3 + j].nextNodes)
                        {
                            MapManager.Instance.mapNodes[i][j].nextNodes.Add(MapManager.Instance.mapNodes[nextNode.x][nextNode.y]);
                        }
                        MapManager.Instance.mapNodes[i][j].transform.position = jsonData.mapData.mapNodes[i * 3 + j].position;
                        MapManager.Instance.mapNodes[i][j].Renderer.color = jsonData.mapData.mapNodes[i * 3 + j].color;
                        if (jsonData.mapData.mapNodes[i * 3 + j].leftNode != new Vector2Int(-1, -1))//左节点
                        {
                            MapManager.Instance.mapNodes[i][j].leftNode = MapManager.Instance.mapNodes[jsonData.mapData.mapNodes[i * 3 + j].leftNode.x][jsonData.mapData.mapNodes[i * 3 + j].leftNode.y];
                        }
                        else
                        {
                            MapManager.Instance.mapNodes[i][j].leftNode = null;
                        }
                        if (jsonData.mapData.mapNodes[i * 3 + j].rightNode != new Vector2Int(-1, -1))//右节点
                        {
                            MapManager.Instance.mapNodes[i][j].rightNode = MapManager.Instance.mapNodes[jsonData.mapData.mapNodes[i * 3 + j].rightNode.x][jsonData.mapData.mapNodes[i * 3 + j].rightNode.y];
                        }
                        else
                        {
                            MapManager.Instance.mapNodes[i][j].rightNode = null;
                        }
                        MapManager.Instance.mapNodes[i][j].nodeType = jsonData.mapData.mapNodes[i * 3 + j].nodeType;
                        MapManager.Instance.mapNodes[i][j].battleNodeInfoName = jsonData.mapData.mapNodes[i * 3 + j].battleNodeInfoName;
                    }
                }
            }

            for (int i = 0; i < MapManager.Instance.mapNodes.Length; i++)
            {
                for (int j = 0; j < MapManager.Instance.mapNodes[i].Length; j++)
                {
                    if (MapManager.Instance.mapNodes[i][j] != null)
                    {
                        MapManager.Instance.mapNodes[i][j].PathGenerate();
                    }
                }
            }

            MapManager.Instance.currAtlasID = jsonData.mapData.currAtlasID;
        }
    
    }


    //是否存在json文件
    public bool ExistJson()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            AssetDatabase.Refresh();
        }
        return File.Exists(JsonPath());
    }


    //json文件路径
    private string JsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Data.json");
    }


    //删除存档文件
    public void DeleteSave()
    {
        if (ExistJson())
        {
            File.Delete(JsonPath());
            Debug.Log("存档删除成功");
        }
        else
        {
            Debug.Log("没有找到存档");
        }
    }
}
