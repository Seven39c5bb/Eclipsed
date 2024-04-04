using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class JsonData
{
    public MapData mapData;
}
[System.Serializable]
public class MapData
{
    public bool mapBeCreated;
    public List<MapNode> mapNodes;
    public List<bool> lockedNodes;
}
public class SaveManager : MonoBehaviour
{
    public JsonData jsonData;
    public static SaveManager instance;
    private void Awake()
    {
        instance = this;
        InitJsonData();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {

    }
    //初始化数据
    void InitJsonData()
    {
        jsonData = new JsonData();
        jsonData.mapData = new MapData();
        jsonData.mapData.mapNodes= new List<MapNode>();
        jsonData.mapData.lockedNodes = new List<bool>();
    }
    //保存数据
    public void Save()
    {
        //test
        Debug.Log(MapManager.Instance.mapNodes[0][1].nodeId);
        //test
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
        //clearlist,防止添加两次数据
        jsonData.mapData.mapNodes.Clear();
        jsonData.mapData.lockedNodes.Clear();
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
    //更新当前数据
    private void UpdateCurDate()
    {
        jsonData.mapData.mapBeCreated = MapManager.Instance.MapBeCreated;
        //将mapmanager中的mapnodes拷贝到jsondata中
        for(int i=0;i<MapManager.Instance.mapNodes.Length;i++)
        {
            for(int j = 0; j < MapManager.Instance.mapNodes[i].Length;j++)
            {
                if (MapManager.Instance.mapNodes[i][j] != null)
                {
                    jsonData.mapData.mapNodes.Add(MapManager.Instance.mapNodes[i][j]);
                }               
            }
        }
        //记录结点解锁状态
        foreach(var node in jsonData.mapData.mapNodes)
        {
            jsonData.mapData.lockedNodes.Add(node.isLocked);
        }
        
    }
    //更新数据
    private void UpdateInfo()
    {
        MapManager.Instance.MapBeCreated = jsonData.mapData.mapBeCreated;
        for(int i=0;i<jsonData.mapData.mapNodes.Count;i++)
        {
            jsonData.mapData.mapNodes[i].isLocked = jsonData.mapData.lockedNodes[i];
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
}
