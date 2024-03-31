using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public MapNode[][] mapNodes;

    private void Awake()
    {
        Instance = this;
        mapNodes = new MapNode[13][];
        for (int i = 0; i < mapNodes.Length; i++)
        {
            mapNodes[i] = new MapNode[3];
        }
    }

    private bool MapBeCreated = false;

    void Update()
    {
        if (!MapBeCreated)
        {
            MapBeCreated = true;
            
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

            // (12,1)为终点，其前驱节点为(11,0),(11,1),(11,2)
            mapNodes[11][0].nextNodes.Add(mapNodes[12][1]);
            mapNodes[11][1].nextNodes.Add(mapNodes[12][1]);
            mapNodes[11][2].nextNodes.Add(mapNodes[12][1]);

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


            // 遍历所有节点，根据后继节点生成路径
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (mapNodes[i][j] == null)
                        continue;
                    mapNodes[i][j].PathGenerate();
                }
            }
        }
    }

    
}
