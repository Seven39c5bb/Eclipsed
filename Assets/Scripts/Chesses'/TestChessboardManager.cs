using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChessboardManager : MonoBehaviour
{
    public ChessboardManager chessboardManager; // Assign this in the Inspector

    void Start()
    {
        TestMoveControl();
    }

    void TestMoveControl()
    {
        // 假设你已经有了一些游戏对象标记为Player和Enemy，并且棋盘已初始化

        // 测试案例1: 玩家向右移动3步
        Debug.Log("测试案例1: 玩家向右移动");
        var player = GameObject.FindGameObjectWithTag("Player");
        var result1 = chessboardManager.MoveControl(player, new Vector2(0, 0), new Vector2(3, 0));
        Debug.Log($"目标位置: {result1.Item1}, 遇到障碍: {result1.Item2}");

        // 测试案例2: 敌人向上移动4步
        Debug.Log("测试案例2: 敌人向下移动");
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        var result2 = chessboardManager.MoveControl(enemy, new Vector2(5, 5), new Vector2(0, 4));
        Debug.Log($"目标位置: {result2.Item1}, 遇到障碍: {result2.Item2}");

        // 添加更多测试用例，以覆盖各种边界情况和可能的障碍类型
    }
}
