using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChessboardManager : MonoBehaviour
{
    GameObject Enemy;
    EnemyBase EnemyController;
    void Start()
    {
        //TestMoveControl();
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        EnemyController = Enemy.GetComponent<EnemyBase>();
    }

    void TestMoveControl()
    {
        // 假设你已经有了一些游戏对象标记为Player和Enemy，并且棋盘已初始化

        // 测试案例1: 玩家向右移动3步
        /* Debug.Log("测试案例1: 玩家向下移动");
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerScript = player.GetComponent<ChessBase>();
        playerScript.Move(new Vector2Int(0, 4)); */


        /* // 测试案例2: 敌人向上移动4步
        Debug.Log("测试案例2: 敌人向下移动");
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        var enemyScript = enemy.GetComponent<ChessBase>();
        enemyScript.Move(new Vector2Int(0, 4));
        // 添加更多测试用例，以覆盖各种边界情况和可能的障碍类型 */
    }

    public bool testEnemyOnTurn = false;
    public bool testEnemyInjury = false;
    void Update()
    {
        if (testEnemyOnTurn)
        {
            testEnemyOnTurn = false;
            EnemyController.StartCoroutine(EnemyController.OnTurn());
        }
        if (testEnemyInjury)
        {
            testEnemyInjury = false;
            EnemyController.TakeDamage(10, PlayerController.instance);
            
        }
    }
}
