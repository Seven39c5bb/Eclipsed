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
    //��ʼ������
    void InitJsonData()
    {
        jsonData = new JsonData();
        jsonData.mapData = new MapData();
        jsonData.mapData.mapNodes= new List<MapNode>();
        jsonData.mapData.lockedNodes = new List<bool>();
    }
    //��������
    public void Save()
    {
        //test
        Debug.Log(MapManager.Instance.mapNodes[0][1].nodeId);
        //test
        UpdateCurDate();
        if (!ExistJson())
        {
            Debug.Log("�����ļ�������λ��Ϊ��" + Application.persistentDataPath);
            File.Create(JsonPath()).Close();
            AssetDatabase.Refresh();
        }
        string json = JsonUtility.ToJson(jsonData, true);
        File.WriteAllText(JsonPath(), json);
        Debug.Log("����ɹ�");
        //clearlist,��ֹ�����������
        jsonData.mapData.mapNodes.Clear();
        jsonData.mapData.lockedNodes.Clear();
    }
    //��ȡ����
    public void Load()
    {
        if (ExistJson())
        {
            string json = File.ReadAllText(JsonPath());
            jsonData = JsonUtility.FromJson<JsonData>(json);
            Debug.Log("��ȡ�ɹ�");
            UpdateInfo();
        }
        else
        {
            Debug.Log("��ȡʧ��");
        }
    }
    //���µ�ǰ����
    private void UpdateCurDate()
    {
        jsonData.mapData.mapBeCreated = MapManager.Instance.MapBeCreated;
        //��mapmanager�е�mapnodes������jsondata��
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
        //��¼������״̬
        foreach(var node in jsonData.mapData.mapNodes)
        {
            jsonData.mapData.lockedNodes.Add(node.isLocked);
        }
        
    }
    //��������
    private void UpdateInfo()
    {
        MapManager.Instance.MapBeCreated = jsonData.mapData.mapBeCreated;
        for(int i=0;i<jsonData.mapData.mapNodes.Count;i++)
        {
            jsonData.mapData.mapNodes[i].isLocked = jsonData.mapData.lockedNodes[i];
        }
    }
    //�Ƿ����json�ļ�
    public bool ExistJson()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            AssetDatabase.Refresh();
        }
        return File.Exists(JsonPath());
    }
    //json�ļ�·��
    private string JsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Data.json");
    }
}
