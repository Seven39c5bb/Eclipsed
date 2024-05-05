using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyTurn : FightUnit
{
    public List<EnemyBase> enemyList = new List<EnemyBase>();

    public override void Init()
    {
        //玩家回合结束时，检查玩家buff
        foreach (var buff in PlayerController.instance.buffList)
        {
            buff.OnTurnEnd();
        }
        FightUI.instance.isEnemyTurn = true;
        Debug.Log("Enemy Turn Now");
        //将手牌中的牌全部移除
        //test

        foreach(Card card in FightUI.cardList)
        {
            CardManager.discardDesk.Add(card.name);
            card.GetComponent<RectTransform>().DOMove(GameObject.Find("discardDesk").transform.position, 0.2f).OnComplete(() =>
            {
                UseCard.instance.RemoveAllCards();
            });            
            //UseCard.instance.Discard(card.gameObject);
            
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

        //遍历所有敌人，衰减其护盾
        foreach (var enemy in enemyList)
        {
            enemy.BarrierDecay();
        }
    }

    private Coroutine currCoroutine = null;//用于检测当前怪物是否已经完成了行动
    private float timer = 0.8f;
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
            timer = 0.8f;
        }
        else if(timer >= 0)
        {
            if (currCoroutine == null) timer -= Time.deltaTime;//等待所有动画播放完毕
        }
        else
        {
            Debug.Log("All Enemy Turn Finished");
            //isInit = false;
            if (currCoroutine == null) FightManager.instance.ChangeType(FightType.Player);
        }
    }

    IEnumerator OnTurnCoroutine(EnemyBase enemy)
    {
        enemyList[0].isActed = true;
        Debug.Log("Current Enemy Turn Started");
        yield return enemy.OnTurn();
        enemyList[0].isActed = false;
        Debug.Log("Current Enemy Turn Finished");
        enemyList.RemoveAt(0);
        currCoroutine = null;
    }
}
