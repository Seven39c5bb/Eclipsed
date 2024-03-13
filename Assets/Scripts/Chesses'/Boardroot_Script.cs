using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boardroot_Script : MonoBehaviour
{
    public GameObject cellPrefab; // 格子预制体
    public int width = 10; // 棋盘的宽度
    public int height = 10; // 棋盘的高度

    // 脚本启动时调用
    void Start()
    {
        CreateChessboard();
    }

    // 创建棋盘的方法
    void CreateChessboard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateCell(x, y);
            }
        }
    }

    // 创建单个格子并设置坐标的方法
    void CreateCell(int x, int y)
    {
        // 实例化格子
        GameObject cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, transform);
        
        // 设置格子的名称，以便于识别其坐标
        cell.name = string.Format("Cell ({0},{1})", x, height - y - 1);
        
        /* // 如果需要，可以添加额外的逻辑，如给格子添加坐标组件
        TileCoordinates coordinates = tile.AddComponent<TileCoordinates>();
        coordinates.x = x;
        coordinates.y = y; */
    }
}
