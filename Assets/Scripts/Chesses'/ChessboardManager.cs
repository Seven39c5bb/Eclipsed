using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;

public class ChessboardManager : MonoBehaviour
{
    public Cell[,] cellStates = new Cell[10, 10];
    public Vector2Int boardSize = new Vector2Int(10, 10);

    //敌方棋子列表
    public List<GameObject> enemyList;
    public List<EnemyBase> enemyControllerList;

    public static ChessboardManager instance;

    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                string cellName = $"Cell ({i},{j})";
                GameObject CellObject = GameObject.Find(cellName);
                cellStates[i, j] = CellObject.GetComponent<Cell>();
            }
        }

        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        foreach (GameObject enemy in enemyList)
        {
            enemyControllerList.Add(enemy.GetComponent<EnemyBase>());
        }

        instance = this;
    }
    

    /// <summary>
    /// 移动指定的游戏对象到指定的位置。
    /// </summary>
    /// <param name="requestObject">要移动的游戏对象(tag:Player or Enemy)。</param>
    /// <param name="location">游戏对象的当前棋格坐标编号。</param>
    /// <param name="direction">游戏对象应该移动的方向和距离。(正方向：右下）</param>
    /// <returns>一个元组：(目标棋格所在坐标(实际), 目标棋格棋盘坐标, 游戏对象遇到的障碍的类型, 遇到的棋子类游戏对象)。可能返回的障碍类型（字符串）："None""Wall""Enemy""Player"</returns>
    public (Vector2, Vector2Int, string, GameObject) MoveControl(GameObject requestObject, Vector2Int location, Vector2Int direction)
    {
        int x = (int)location.x;
        int y = (int)location.y;
        int dx = (int)direction.x;
        int dy = (int)direction.y;

        Vector2Int target_location = location;//移动目标棋格,lacation为当前棋格
        Vector2 target_position = requestObject.transform.position;//移动目标位置

        string roadblockType = "None";//障碍物类型
        GameObject roadblockObject = null;//障碍物对象
        
        
        if(dy == 0)//当是在x上移动时
        {   
            int step = dx > 0 ? 1 : -1;
            dx = Math.Abs(dx);
            
            for(int i = 1; i <= dx; i++)
            {

                int xi = x + i * step;
                if(xi > 9 || xi < 0)//当路径超出棋盘时
                {
                    target_location = new Vector2Int(xi - step, y);
                    roadblockType = "Wall";
                    //调整棋格状态
                    cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                    cellStates[x + (i - 1) * step, y].occupant = requestObject;
                    if(x + (i - 1) * step != x)//除非目的地不是原地,才将原地状态改为空
                    {
                        cellStates[x , y].state = Cell.StateType.Empty;
                    }
                    break;
                }


                if(cellStates[x + i * step, y].state != Cell.StateType.Empty)//当路径上有障碍时
                {
                    target_location = new Vector2Int(x + (i - 1) * step, y);//目标位置

                    if(cellStates[x + i * step, y].state == Cell.StateType.Wall)//障碍为墙时
                    {
                        //遇到墙在走到墙前时要弹出UI提示（感叹号？）
                        roadblockType = "Wall";
                        //调整棋格状态
                        cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                        cellStates[x + (i - 1) * step, y].occupant = requestObject;
                        if(x + (i - 1) * step != x)//除非目的地不是原地,才将原地状态改为空
                        {
                            cellStates[x , y].state = Cell.StateType.Empty;
                        }
                        break;
                    }

                    if(cellStates[x + i * step, y].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x + i * step, y].occupant.tag == "Enemy")//障碍是敌方棋子时
                        {
                            cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                            cellStates[x + (i - 1) * step, y].occupant = requestObject;
                            if(x + (i - 1) * step != x)//除非目的地不是原地,才将原地状态改为空
                            {
                                cellStates[x , y].state = Cell.StateType.Empty;
                            }
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                        if(cellStates[x + i * step, y].occupant.tag == "Player")//障碍是玩家时
                        {
                            target_location = new Vector2Int(x + (i - 1) * step, y);
                            cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                            cellStates[x + (i - 1) * step, y].occupant = requestObject;
                            if(x + (i - 1) * step != x)//除非目的地不是原地,才将原地状态改为空
                            {
                                cellStates[x , y].state = Cell.StateType.Empty;
                            }
                            roadblockType = "Player";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                    }
                }

                if(i == dx)//当畅通无阻时
                {
                    target_location = new Vector2Int(x + dx * step, y);
                    cellStates[x + dx * step, y].state = Cell.StateType.Occupied;
                    cellStates[x + dx * step, y].occupant = requestObject;
                    cellStates[x , y].state = Cell.StateType.Empty;
                }
            }
        }
        else//当是在y上移动时(与x上移动类似)
        {
            int step = dy > 0 ? 1 : -1;
            dy = Math.Abs(dy);
            
            for(int i = 1; i <= dy; i++)
            {
                int yi = y + i * step;
                if(yi > 9 || yi < 0)
                {
                    target_location = new Vector2Int(x, yi - step);
                    roadblockType = "Wall";
                    cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                    cellStates[x, y + (i - 1) * step].occupant = requestObject;
                    if(y + (i - 1) * step != y)//除非目的地不是原地，才将原地状态改为空
                    {
                        cellStates[x , y].state = Cell.StateType.Empty;
                    }
                    break;
                }

                if(cellStates[x, y + i * step].state != Cell.StateType.Empty)
                {
                    target_location = new Vector2Int(x, y + (i - 1) * step);
                    if(cellStates[x, y + i * step].state == Cell.StateType.Wall)
                    {
                        roadblockType = "Wall";
                        cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                        cellStates[x, y + (i - 1) * step].occupant = requestObject;
                        if(y + (i - 1) * step != y)//除非目的地不是原地，才将原地状态改为空
                        {
                            cellStates[x , y].state = Cell.StateType.Empty;
                        }
                        break;
                    }
                    if(cellStates[x, y + i * step].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x, y + i * step].occupant.tag == "Enemy")//障碍是敌方棋子时
                        {
                            cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                            cellStates[x, y + (i - 1) * step].occupant = requestObject;
                            if(y + (i - 1) * step != y)//除非目的地不是原地，才将原地状态改为空
                            {
                                cellStates[x , y].state = Cell.StateType.Empty;
                            }
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                        if(cellStates[x, y + i * step].occupant.tag == "Player")//障碍是玩家时
                        {
                            target_location = new Vector2Int(x, y + (i - 1) * step);
                            cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                            cellStates[x, y + (i - 1) * step].occupant = requestObject;
                            if(y + (i - 1) * step != y)//除非目的地不是原地，才将原地状态改为空
                            {
                                cellStates[x , y].state = Cell.StateType.Empty;
                            }
                            roadblockType = "Player";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                    }
                }

                if(i == dy)//当畅通无阻时
                {
                    target_location = new Vector2Int(x, y + dy * step);
                    cellStates[x, y + dy * step].state = Cell.StateType.Occupied;
                    cellStates[x, y + dy * step].occupant = requestObject;
                    cellStates[x , y].state = Cell.StateType.Empty;
                }
            }
        }
        target_position = cellStates[(int)target_location.x, (int)target_location.y].transform.position + new Vector3(0, 0.7f, 0);

        return (target_position, target_location, roadblockType, roadblockObject);
    }











    public class Node
    {
        public int X;
        public int Y;
        public Node Parent;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    private readonly int[] dx = { -1, 0, 1, 0 };
    private readonly int[] dy = { 0, 1, 0, -1 };
    /// <summary>
    /// 从起点到终点寻找路径。
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="end">终点</param>
    /// <param name="aimObject">目标对象</param>
    /// <param name="areaFunc">计算范围的函数，该函数输入一个Vector2Int，输出一个Vector2Int数组。注意设计该函数时要排除墙和被占用的格子</param>
    /// <returns>返回一整个路径，包括自身所在的起点以及终点</returns>
    /// <remarks>!!!!  设计范围函数时记得排除墙和被占用的格子，防止出bug（不包括怪物自己所在格子，否则会反复横跳）  !!!!</remarks>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, GameObject aimObject, Func<Vector2Int, Vector2Int[]> areaFunc = null)
    {
        //创建一个Vector2Int数组，用于存储玩家周围的的怪物偏好区
        Vector2Int[] CellsInRange;
        if(areaFunc != null)
        {
            CellsInRange = areaFunc(end);
        }
        else
        {
            CellsInRange = new Vector2Int[1];
            CellsInRange[0] = end;
        }


        Node[,] Nodes = new Node[boardSize.x, boardSize.y];//创建一个节点数组，用于BFS
        for (int i = 0; i < boardSize.x; i++)
        {
            for (int j = 0; j < boardSize.y; j++)
            {
                Nodes[i, j] = new Node(i, j);
            }
        }


        bool[,] visited = new bool[boardSize.x, boardSize.y];//创建一个bool数组，用于记录节点是否被访问过
        Queue<Node> queue = new Queue<Node>();

        visited[start.x, start.y] = true;
        queue.Enqueue(Nodes[start.x, start.y]);

        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            //判断是否找到终点(终点可能是一整个范围)

            //找到CellsInRange中离当前节点最近的点
            foreach (Vector2Int cell in CellsInRange)
            {
                //找出CellsInRange中离当前节点最近的点
                //计算当前节点到cell的距离
                int distance = Math.Abs(cell.x - node.X) + Math.Abs(cell.y - node.Y);
                int minDistance = 9999;
                if(distance < minDistance)//找到最近的点
                {
                    minDistance = distance;
                    end = cell;
                }
                
            }

            if (node.X == end.x && node.Y == end.y)//找到终点,返回路径
            {
                List<Vector2Int> path = new List<Vector2Int>();
                while (node != null)
                {
                    path.Add(new Vector2Int(node.X, node.Y));
                    node = node.Parent;
                }
                path.Reverse();
                return path;
            }

            for (int i = 0; i < 4; i++)
            {
                //让搜索方向随机
                int randomIndex = UnityEngine.Random.Range(i, 4);
                // Swap dx[i] and dx[randomIndex]
                int temp = dx[i];
                dx[i] = dx[randomIndex];
                dx[randomIndex] = temp;
                // Swap dy[i] and dy[randomIndex]
                temp = dy[i];
                dy[i] = dy[randomIndex];
                dy[randomIndex] = temp;

                int newX = node.X + dx[i];
                int newY = node.Y + dy[i];

                if (newX >= 0 && newX < boardSize.x && newY >= 0 && newY < boardSize.y 
                    && cellStates[newX, newY].state != Cell.StateType.Wall 
                    && (cellStates[newX, newY].state != Cell.StateType.Occupied || cellStates[newX, newY].occupant == aimObject) 
                    && !visited[newX, newY])//判断是否越界、是否是墙、是否被占用(玩家的占用不算，否则会找不到路径)、是否被访问过
                {
                    visited[newX, newY] = true;
                    Nodes[newX, newY].Parent = node;
                    queue.Enqueue(Nodes[newX, newY]);
                }
            }
        }

        Debug.Log("Finding path from " + start + " to " + end);

        // ... rest of your code ...

        if (queue.Count == 0)
        {
            Debug.Log("No path found from " + start + " to " + end);
            return null;
        }

        return null;//没有找到路径
    }
















    /// <summary>
    /// 从棋盘上移除指定的棋子（仅仅改变棋格属性）。
    /// </summary>
    /// <param name="deleteObject">要删除的棋子对象。</param>
    public void RemoveChess(GameObject deleteObject)
    {
        //遍历所有棋格，找到要删除的对象
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(cellStates[i, j].occupant == deleteObject)
                {
                    cellStates[i, j].state = Cell.StateType.Empty;
                    cellStates[i, j].occupant = null;
                    //更新敌方棋子列表和敌方控制器列表中中删除
                    enemyList.Remove(deleteObject);
                    enemyControllerList.Remove(deleteObject.GetComponent<EnemyBase>());
                    Destroy(deleteObject);
                }
            }
        }

        
    }

    /// <summary>
    /// 往棋盘上添加指定的棋子（仅仅改变棋格属性）。
    /// </summary>
    /// <param name="addObject">要添加的棋子对象。</param>
    /// <param name="Location">要添加的棋子的位置。</param>
    /// <remarks>添加棋子时需要判断棋格是否为空，如果不为空则不添加。</remarks>
    /// <returns>返回是否添加成功。</returns>
    public bool AddChess(GameObject addObject, Vector2Int Location)
    {
        if(cellStates[Location.x, Location.y].state == Cell.StateType.Empty)
        {
            cellStates[Location.x, Location.y].state = Cell.StateType.Occupied;
            cellStates[Location.x, Location.y].occupant = addObject;
            addObject.transform.position = cellStates[Location.x, Location.y].transform.position + new Vector3(0, 0.7f, 0);

            //更新敌方棋子列表
            enemyList.Clear();
            enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            enemyControllerList.Clear();
            foreach (GameObject enemy in enemyList)//要考虑List中的对象可能已经被销毁
            {
                if(!UnityEngine.Object.Equals(enemy, null))
                {
                    enemyControllerList.Add(enemy.GetComponent<EnemyBase>());
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    
    /// <summary>
    /// 检查指定的棋格是否有棋子。
    /// </summary>
    /// <param name="Location">要检查的棋格坐标。</param>
    /// <returns>返回棋格上的棋子对象，如果没有则返回null。</returns>
    /// <remarks>返回的棋子对象可能是玩家或敌方棋子,需要在其他脚本中处理</remarks>
    public ChessBase CheckCell(Vector2Int Location)
    {
        if(cellStates[Location.x, Location.y].state == Cell.StateType.Occupied)
        {
            return cellStates[Location.x, Location.y].occupant.GetComponent<ChessBase>();
        }
        else
        {
            return null;
        }
        
    }

}
