using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;

public class ChessboardManager : MonoBehaviour
{
    public Cell[,] cellStates = new Cell[10, 10];
    public Vector2Int boardSize = new Vector2Int(10, 10);
    //选中的cell
    public Cell curCell;

    //敌方棋子列表
    public List<GameObject> enemyList;
    public List<EnemyBase> enemyControllerList;

    public static ChessboardManager Chess_instance;
    public static ChessboardManager instance
    {
        get
        {
            if(Chess_instance== null)
            {
                Chess_instance = GameObject.FindObjectOfType<ChessboardManager>();
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        string cellName = $"Cell ({i},{j})";
                        GameObject CellObject = GameObject.Find(cellName);
                        ChessboardManager.Chess_instance.cellStates[i, j] = CellObject.GetComponent<Cell>();
                        ChessboardManager.Chess_instance.cellStates[i, j].cellLocation = new Vector2Int(i, j);
                    }
                }

                ChessboardManager.Chess_instance.UpdateEnemyControllerList();
            }
            return Chess_instance;
        }
    }

    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                string cellName = $"Cell ({i},{j})";
                GameObject CellObject = GameObject.Find(cellName);
                cellStates[i, j] = CellObject.GetComponent<Cell>();
                cellStates[i, j].cellLocation = new Vector2Int(i, j);
            }
        }
        UpdateEnemyControllerList();

        Chess_instance = this;

        //ChangeProperty(new Vector2Int(5, 4), "Smoke");
    }

    public void UpdateEnemyControllerList()
    {
        enemyList.Clear();
        enemyControllerList.Clear();
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            if(enemy != null && enemy.activeInHierarchy)
            {
                enemyList.Add(enemy);
                enemyControllerList.Add(enemy.GetComponent<EnemyBase>());
            }
        }
    }
    

    /// <summary>
    /// 移动指定的游戏对象到指定的位置。
    /// </summary>
    /// <param name="requestObject">要移动的游戏对象(tag:Player or Enemy)。</param>
    /// <param name="location">游戏对象的当前棋格坐标编号。</param>
    /// <param name="direction">游戏对象应该移动的方向和距离。(正方向：右下）</param>
    /// <returns>一个列表，包含这次移动请求包含的所有移动命令</returns>
    public List<IMobileUnit> MoveControl(GameObject requestObject, Vector2Int location, Vector2Int direction)
    {
        int x = (int)location.x;
        int y = (int)location.y;
        int dx = (int)direction.x;
        int dy = (int)direction.y;

        Vector2Int target_location = location;//移动目标棋格,lacation为当前棋格

        string roadblockType = "None";//障碍物类型
        GameObject roadblockObject = null;//障碍物对象
        List<IMobileUnit> mobileUnits = new List<IMobileUnit>();//棋子需要执行的移动操作和近战攻击命令集
        
        
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
                    break;
                }


                if(cellStates[x + i * step, y].state != Cell.StateType.Empty)//当路径上有障碍时
                {
                    target_location = new Vector2Int(x + (i - 1) * step, y);//目标位置

                    if(cellStates[x + i * step, y].state == Cell.StateType.Wall)//障碍为墙时
                    {
                        //遇到墙在走到墙前时要弹出UI提示（感叹号？）
                        roadblockType = "Wall";
                        break;
                    }

                    if(cellStates[x + i * step, y].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x + i * step, y].occupant.GetComponent<EnemyBase>())//障碍是敌方棋子时
                        {
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                        if(cellStates[x + i * step, y].occupant.tag == "Player")//障碍是玩家时
                        {
                            roadblockType = "Player";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                    }
                }

                mobileUnits.Add(new MoveUnit(requestObject.GetComponent<ChessBase>(), new Vector2Int(step, 0)));//添加移动操作

                if(i == dx)//当畅通无阻时
                {
                    target_location = new Vector2Int(x + dx * step, y);
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
                    break;
                }

                if(cellStates[x, y + i * step].state != Cell.StateType.Empty)
                {
                    target_location = new Vector2Int(x, y + (i - 1) * step);
                    if(cellStates[x, y + i * step].state == Cell.StateType.Wall)
                    {
                        roadblockType = "Wall";
                        break;
                    }
                    if(cellStates[x, y + i * step].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x, y + i * step].occupant.tag == "Enemy")//障碍是敌方棋子时
                        {
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                        if(cellStates[x, y + i * step].occupant.tag == "Player")//障碍是玩家时
                        {
                            roadblockType = "Player";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                    }
                }

                mobileUnits.Add(new MoveUnit(requestObject.GetComponent<ChessBase>(), new Vector2Int(0, step)));//添加移动操作

                if(i == dy)//当畅通无阻时
                {
                    target_location = new Vector2Int(x, y + dy * step);
                }
            }
        }


        //计算Location和aimLocation之间的距离
        int residualDistance = Math.Abs(direction.x) + Math.Abs(direction.y) - Math.Abs(target_location.x - location.x) - Math.Abs(target_location.y - location.y);

        //如果有剩余距离，说明可能需要执行近战攻击
        if(residualDistance > 0)
        {
            if (requestObject.GetComponent<ChessBase>().dontMeleeAttack == false && roadblockObject != null)
            {
                //根据该棋子的不同分类，对不同的障碍物做出不同的处理
                switch (roadblockType)
                {
                    case "Enemy":
                        if(requestObject.tag == "Player")
                        {
                            //添加攻击敌人的操作
                            mobileUnits.Add(new MeleeAttackUnit(requestObject.GetComponent<ChessBase>(), roadblockObject.GetComponent<ChessBase>(), residualDistance));
                        }
                        break;
                    case "Player":
                        if(requestObject.tag == "Enemy")
                        {
                            //添加攻击玩家的操作
                            mobileUnits.Add(new MeleeAttackUnit(requestObject.GetComponent<ChessBase>(), roadblockObject.GetComponent<ChessBase>(), residualDistance));
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return mobileUnits;
    }


    /// <summary>
    /// 将指定棋子移动到指定位置。
    /// </summary>
    /// <param name="requestObject">要移动的棋子对象。</param>
    /// <param name="Location">要移动到的位置。</param>
    /// <returns>返回是否移动成功。</returns>
    /// <remarks>移动成功后会更新棋盘信息。</remarks>
    public bool MoveChess(GameObject requestObject, Vector2Int Location)
    {
        if(cellStates[Location.x, Location.y].state == Cell.StateType.Empty)
        {
            cellStates[Location.x, Location.y].state = Cell.StateType.Occupied;
            cellStates[Location.x, Location.y].occupant = requestObject;
            cellStates[requestObject.GetComponent<ChessBase>().location.x, requestObject.GetComponent<ChessBase>().location.y].state = Cell.StateType.Empty;
            requestObject.GetComponent<ChessBase>().location = Location;//更新请求棋子的Location
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 传送请求，检查并更改指定位置棋格以及请求棋子的信息，预传送
    /// </summary>
    /// <param name="requestObject">请求传送的棋子对象。</param>
    /// <param name="Location">传送目标位置。</param>
    /// <returns>返回是否传送成功。</returns>
    /// <remarks>请求后就已经更改信息，需要在请求出更改棋子的世界position。</remarks>
    public bool TeleportRequest(ChessBase requestObject, Vector2Int Location)
    {
        if(cellStates[Location.x, Location.y].state == Cell.StateType.Empty)
        {
            cellStates[Location.x, Location.y].state = Cell.StateType.Occupied;
            cellStates[Location.x, Location.y].occupant = requestObject.gameObject;
            cellStates[requestObject.location.x, requestObject.location.y].state = Cell.StateType.Empty;
            cellStates[requestObject.location.x, requestObject.location.y].occupant = null;
            requestObject.location = Location;//更新请求棋子的Location
            return true;
        }
        else
        {
            return false;
        }
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

        if (CellsInRange == null || CellsInRange.Length == 0)
        {
            Debug.Log("No cells in range");
            return null;
        }

        Debug.Log("start: " + start);
        //找到CellsInRange中离当前节点最近的点
        int minDistance = 9999;
        foreach (Vector2Int cell in CellsInRange)
        {
            //找出CellsInRange中离当前节点最近的点
            //计算当前节点到cell的距离
            int distance = (cell.x - start.x) * (cell.x - start.x) + (cell.y - start.y) * (cell.y - start.y); 
            if(distance < minDistance)//找到最近的点
            {
                minDistance = distance;
                end = cell;
            }
        }
        Debug.Log("end: " + end);

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
                    StartCoroutine(DelayedUpdate());
                }
            }
        }        
    }
    private IEnumerator DelayedUpdate()
    {
        //等待当前帧结束
        yield return new WaitForEndOfFrame();
        //然后更新敌人列表
        UpdateEnemyControllerList();
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
            addObject.transform.position = cellStates[Location.x, Location.y].transform.position ;

            UpdateEnemyControllerList();
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

    ///<summary>
    ///将棋格替换为另一个棋格
    ///</summary>
    ///<param name="location">要更换的棋格坐标</param>
    ///<param name="className">要进行更换的棋格类型类名</param>
    public void ChangeCell(Vector2Int location, string className)
    {
        int x = location.x, y = location.y;
        GameObject cell = cellStates[x, y].gameObject;
        //添加脚本
        Type type = Type.GetType(className);
        if(type != null)
        {
            Destroy(cell.GetComponent<Cell>());
            cell.AddComponent(type);
            cellStates[x, y] = (Cell)cell.GetComponent(type);
        }
        //更换贴图
    }

    ///<summary>
    ///更换棋格额外属性
    /// </summary>
    public void ChangeProperty(Vector2Int location, string propertyName)
    {
        int x = location.x, y = location.y;
        GameObject cell = cellStates[x, y].gameObject;
        if (cellStates[x, y].property?.propertyName == propertyName) return;
        if(cellStates[x, y].property != null)
        {
            //Debug.Log("测试是否触发OnRemove");
            cellStates[x, y].property.OnRemove();
        }
        if (propertyName == null) return;
        GameObject obj = Instantiate(Resources.Load("Prefabs/CellProperty/"+propertyName), cell.transform) as GameObject;
        
        obj.GetComponent<CellProperty>().cell = cellStates[x, y];
        cellStates[x,y].property=obj.GetComponent<CellProperty>();
        //调用
        cellStates[x, y].property.OnAdd();

        Debug.Log(obj.name);
    }

    ///<summary>
    ///召唤
    ///</summary>
    public void Summon(Vector2Int location,string monsterName)
    {
        //判断是否越界
        if (location.x > 9 || location.y > 9 || location.x < 0 || location.y < 0) return;
        if (ChessboardManager.instance.cellStates[location.x,location.y].state
            == Cell.StateType.Empty)
        {
            GameObject monster = Resources.Load("Prefabs/Chesses/" + monsterName) as GameObject;
            monster.GetComponent<EnemyBase>().location = location;
            Instantiate(monster);
        }
    }
}
