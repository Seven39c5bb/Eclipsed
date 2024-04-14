using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MapNode : MonoBehaviour
{
    // 是否被锁定
    public bool isLocked = true;

    // id
    public Vector2Int nodeId;
    
    // 相邻节点
    public List<MapNode> nextNodes;//后继节点们

    //左右节点意味着与当前节点同层的节点
    public MapNode leftNode;//左侧节点
    public MapNode rightNode;//右侧节点

    // 从节点返回的AtlasID
    public MapManager.AtlasID backAtlasID;//记得BOSS节点需要手动设置该值！！！！！！！！！！！！！！！！！！！！！！！！

    // 节点类型
    public enum NodeType
    {

        // 该部分为随机节点
        Fight,
        Elite,
        Hunting,
        Event,

        // 该部分为固定分节点
        Plot,
        Shop,

        // 该部分为固定终节点
        Boss
    }

    public NodeType nodeType; 

    // 储存战斗类节点关卡信息的Txt文件
    public string NodeInfoName;//根据当前节点id确定位置，随机抽取对应战斗布置

    // 跳转的场景
    private string sceneName;//将会从存档文件中读取

    public SpriteRenderer Renderer;

    private Vector3 originLocalScale;

    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        originLocalScale = this.transform.localScale;

        MapManager.Instance.mapNodes[nodeId.x][nodeId.y] = this;

        // 以父对象的位置为中心，半径为1的范围，随机旋转一个位置作为自己的新位置
        float angle = Random.Range(0, 360);
        float radius = Random.Range(0.2f, 1);
        this.transform.position = this.transform.parent.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

        // 被锁定的节点设置为灰色
        if (isLocked)
        {
            Renderer.color = new Color(75f/255f, 75f/255f, 75f/255f, 1);
        }
        // 未被锁定的节点设置为白色
        else
        {
            Renderer.color = new Color(198f/255f, 157f/255f, 1, 1);
        }

        // 生成路径
        //PathGenerate();
    }

    private void OnMouseEnter()
    {
        if (isLocked)
            return;
        
        // 放大节点
        this.transform.localScale = this.transform.localScale * 1.2f;

    }

    private void OnMouseOver()
    {
        if (isLocked)
            return;

        // 玩家点击该节点时
        if (Input.GetMouseButtonDown(0))
        {
            // 先将节点调成黄色，然后再调整回来
            Renderer.DOColor(Color.yellow, 0.05f).OnComplete(() =>
            {
                Renderer.DOColor(new Color(198f/255f, 157f/255f, 1, 1), 0.05f).OnComplete(() =>
                {
                    this.transform.localScale = originLocalScale;// 还原节点的大小
                    SaveManager.instance.jsonData.mapData.currNodeID = nodeId;//将当前节点id存入存档
                    SaveManager.instance.Save();//保存存档
                    switch (nodeType)
                    {
                        case NodeType.Fight:
                            // 进入战斗场景
                            SaveManager.instance.jsonData.mapData.currNodeInfoName = NodeInfoName;//用于战斗场景怪物的初始化
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;//用于战斗胜利后的返回
                            SaveManager.instance.Save();//保存存档
                            SceneManager.LoadScene("CardTest");
                            break;
                        case NodeType.Elite:
                            // 进入精英战斗场景
                            SaveManager.instance.jsonData.mapData.currNodeInfoName = NodeInfoName;
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            SceneManager.LoadScene("CardTest");
                            break;
                        case NodeType.Hunting:
                            // 进入狩猎场景
                            SaveManager.instance.jsonData.mapData.currNodeInfoName = NodeInfoName;
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            break;
                        case NodeType.Event:
                            // 进入事件场景
                            SaveManager.instance.jsonData.mapData.currNodeInfoName = NodeInfoName;
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            SceneManager.LoadScene("Event");
                            break;
                        case NodeType.Plot:
                            // 进入剧情场景
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            VisitNode();
                            break;
                        case NodeType.Shop:
                            // 进入商店场景
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            break;
                        case NodeType.Boss:
                            // 进入Boss战斗场景
                            SaveManager.instance.jsonData.mapData.currNodeInfoName = NodeInfoName;
                            SaveManager.instance.jsonData.mapData.backAtlasID = backAtlasID;
                            SaveManager.instance.Save();
                            SceneManager.LoadScene("CardTest");
                            break;
                }
                });
                
            });
        }
    }

    private void OnMouseExit()
    {
        if (isLocked)
            return;

        // 还原节点的大小
        this.transform.localScale = originLocalScale;
    }

    // 当从其他场景返回时，将刚刚的节点设置为已探索
    // 战斗胜利时，更新存档中的某个bool变量
    public void VisitNode()
    {
        Renderer.DOColor(Color.white, 0.05f);// 将已经探索过的节点调成白色
        // 将本层的节点（左右节点一直访问直到为空）全部锁定
        MapNode tempNode = this;
        while (tempNode != null)
        {
            tempNode.isLocked = true;
            tempNode.Renderer.color = new Color(75f/255f, 75f/255f, 75f/255f, 1);
            tempNode = tempNode.leftNode;
        }
        tempNode = this.rightNode;
        while (tempNode != null)
        {
            tempNode.isLocked = true;
            tempNode.Renderer.color = new Color(75f/255f, 75f/255f, 75f/255f, 1);
            tempNode = tempNode.rightNode;
        }

        // 将nextNodes中的节点解锁
        foreach (MapNode nextNode in nextNodes)
        {
            nextNode.isLocked = false;
            nextNode.Renderer.color = new Color(198f/255f, 157f/255f, 1, 1);
        }

        SaveManager.instance.Save();//保存存档
    }





    public void PathGenerate()
    {
        // 在该节点和该节点的后继节点之间生成虚线
        foreach (MapNode nextNode in nextNodes)
        {
            GameObject lineObj = new GameObject();
            lineObj.transform.position = this.transform.position;
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            
            // 设置线的属性
            line.material = new Material(Shader.Find("Sprites/Default")); // 设置材质
            line.startColor = line.endColor = Color.grey; // 设置颜色
            line.startWidth = line.endWidth = 0.15f; // 设置宽度
            line.sortingOrder = -1; // 设置渲染顺序

            // 设置虚线
            line.textureMode = LineTextureMode.Tile;
            line.material.mainTextureScale = new Vector2(0.1f, 1f); // 调整这个值可以改变虚线的样式

            // 设置线的位置
            line.positionCount = 2;
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, nextNode.transform.position);
        }
    }

    public void SetNodeSprite()
    {
        // 根据节点类型设置节点的图片
        switch (nodeType)
        {
            case NodeType.Fight:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Fight");
                break;
            case NodeType.Elite:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Elite");
                break;
            case NodeType.Hunting:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Hunting");
                break;
            case NodeType.Event:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Event");
                break;
            case NodeType.Plot:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Plot");
                break;
            case NodeType.Shop:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Shop");
                break;
            case NodeType.Boss:
                //Renderer.sprite = Resources.Load<Sprite>("Sprites/Atlas/Boss");
                break;
        }
    }
}
