using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChessboardManager : MonoBehaviour
{
    private Cell[,] cellStates = new Cell[10, 10];

    // Start is called before the first frame update
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

        //Debug.Log(cellStates[0, 2].state);
        //Debug.Log(cellStates[0, 2].transform.position);
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

        Vector2Int target = location;//移动目标棋格,lacation为当前棋格
        Vector2 target_lacation = requestObject.transform.position;//移动目标位置

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
                    target = new Vector2Int(xi - step, y);
                    roadblockType = "Wall";
                    //调整棋格状态
                    cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                    cellStates[x + (i - 1) * step, y].occupant = requestObject;
                    cellStates[x , y].state = Cell.StateType.Empty;
                    break;
                }


                if(cellStates[x + i * step, y].state != Cell.StateType.Empty)//当路径上有障碍时
                {
                    target = new Vector2Int(x + (i - 1) * step, y);//目标位置

                    if(cellStates[x + i * step, y].state == Cell.StateType.Wall)//障碍为墙时
                    {
                        //遇到墙在走到墙前时要弹出UI提示（感叹号？）
                        roadblockType = "Wall";
                        //调整棋格状态
                        cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                        cellStates[x + (i - 1) * step, y].occupant = requestObject;
                        cellStates[x , y].state = Cell.StateType.Empty;
                        break;
                    }

                    if(cellStates[x + i * step, y].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x + i * step, y].occupant.tag == "Enemy")//障碍是敌方棋子时
                        {
                            cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                            cellStates[x + (i - 1) * step, y].occupant = requestObject;
                            cellStates[x , y].state = Cell.StateType.Empty;
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                        if(cellStates[x + i * step, y].occupant.tag == "Player")//障碍是玩家时
                        {
                            target = new Vector2Int(x + (i - 1) * step, y);
                            cellStates[x + (i - 1) * step, y].state = Cell.StateType.Occupied;
                            cellStates[x + (i - 1) * step, y].occupant = requestObject;
                            cellStates[x , y].state = Cell.StateType.Empty;
                            roadblockType = "Player";
                            roadblockObject = cellStates[x + i * step, y].occupant;
                            break;
                        }
                    }
                }

                if(i == dx)//当畅通无阻时
                {
                    target = new Vector2Int(x + dx * step, y);
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
                    target = new Vector2Int(x, yi - step);
                    roadblockType = "Wall";
                    cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                    cellStates[x, y + (i - 1) * step].occupant = requestObject;
                    cellStates[x , y].state = Cell.StateType.Empty;
                    break;
                }

                if(cellStates[x, y + i * step].state != Cell.StateType.Empty)
                {
                    target = new Vector2Int(x, y + (i - 1) * step);
                    if(cellStates[x, y + i * step].state == Cell.StateType.Wall)
                    {
                        roadblockType = "Wall";
                        cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                        cellStates[x, y + (i - 1) * step].occupant = requestObject;
                        cellStates[x , y].state = Cell.StateType.Empty;
                        break;
                    }
                    if(cellStates[x, y + i * step].state == Cell.StateType.Occupied)
                    {
                        if(cellStates[x, y + i * step].occupant.tag == "Enemy")//障碍是敌方棋子时
                        {
                            cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                            cellStates[x, y + (i - 1) * step].occupant = requestObject;
                            cellStates[x , y].state = Cell.StateType.Empty;
                            roadblockType = "Enemy";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                        if(cellStates[x, y + i * step].occupant.tag == "Player")//障碍是玩家时
                        {
                            target = new Vector2Int(x, y + (i - 1) * step);
                            cellStates[x, y + (i - 1) * step].state = Cell.StateType.Occupied;
                            cellStates[x, y + (i - 1) * step].occupant = requestObject;
                            cellStates[x , y].state = Cell.StateType.Empty;
                            roadblockType = "Player";
                            roadblockObject = cellStates[x, y + i * step].occupant;
                            break;
                        }
                    }
                }

                if(i == dy)//当畅通无阻时
                {
                    target = new Vector2Int(x, y + dy * step);
                    cellStates[x, y + dy * step].state = Cell.StateType.Occupied;
                    cellStates[x, y + dy * step].occupant = requestObject;
                    cellStates[x , y].state = Cell.StateType.Empty;
                }
            }
        }
        target_lacation = cellStates[(int)target.x, (int)target.y].transform.position;

        return (target_lacation, target, roadblockType, roadblockObject);
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
            addObject.transform.position = cellStates[Location.x, Location.y].transform.position;
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
    public GameObject CheckCell(Vector2Int Location)
    {
        if(cellStates[Location.x, Location.y].state == Cell.StateType.Occupied)
        {
            return cellStates[Location.x, Location.y].occupant;
        }
        else
        {
            return null;
        }
        
    }

}
