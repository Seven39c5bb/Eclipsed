using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyTurn : FightUnit
{
    public List<EnemyBase> enemyList = new List<EnemyBase>();

    public override void Init()
    {
        FightUI.instance.isEnemyTurn = true;
        Debug.Log("Enemy Turn Now");
        //将手牌中的牌全部移除
        //test

        foreach(Card card in FightUI.cardList)
        {
            card.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.2f);
            CardManager.discardDesk.Add(card.name);
            //UseCard.instance.Discard(card.gameObject);
            UseCard.instance.Invoke("RemoveAllCards", 0.2f);
        }

        //test
        enemyList.Clear();
        enemyList = new List<EnemyBase>(ChessboardManager.instance.enemyControllerList);//获取所有当前敌人
        Debug.Log("Enemy Count: " + enemyList.Count);
        //打印当前所有敌人脚本的类
        foreach (var enemy in enemyList)
        {
            Debug.Log(enemy.GetType());
        }
    }

    private Coroutine currCoroutine = null;
    public override void OnUpdate()//相当于Update
    {
        //调用第一个个敌人的OnTurn，并等待其完成，将之移出列表
        //Debug.Log("Enemy Count: " + enemyList.Count);
        if (enemyList.Count > 0)
        {
            if (currCoroutine == null)
            {
                Debug.Log("Current Enemy Turn Started");
                currCoroutine = ChessboardManager.instance.StartCoroutine(OnTurnCoroutine(enemyList[0]));//通过下面的协程来调用敌人的OnTurn
                Debug.Log("Current Enemy Turn Started");
            }
        }
        else
        {
            Debug.Log("All Enemy Turn Finished");
            //isInit = false;
            FightManager.instance.ChangeType(FightType.Player);
        }
    }

    IEnumerator OnTurnCoroutine(EnemyBase enemy)
    {
        Debug.Log("Current Enemy Turn Started");
        yield return enemy.OnTurn();
        Debug.Log("Current Enemy Turn Finished");
        enemyList.RemoveAt(0);
        currCoroutine = null;
    }
}
