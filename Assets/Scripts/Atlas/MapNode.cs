using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    // 是否被锁定
    public bool isLocked;

    // id
    public Vector3Int nodeId;
    
    // 相邻节点
    public MapNode preNode;//先驱节点
    public MapNode nextNode;//后继节点
    public MapNode leftNode;//左侧节点
    public MapNode rightNode;//右侧节点

    // 节点类型
    public enum NodeType
    {
        Origin,
        Fight,
        Elite,
        Shop,
        Event,
        Boss
    }

    // 储存战斗类节点关卡信息的Txt文件
    public TextAsset fightNodeInfo;

    // 跳转的场景
    public string sceneName;
}
