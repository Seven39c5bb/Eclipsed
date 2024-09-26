using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFleshLoad_BloodSoup : BuffBase
{
    void Awake()
    {
        buffNameCN = "血肉上膛";
        description = "血肉已经上膛，准备发射！";
        durationTurn = 9999;
        buffType = BuffType.Buff;
        buffImgType = BuffImgType.Damage;
    }
    public Vector2Int playerPrePos;
    public GameObject BloodSoup_AimBar;
    public MeshRenderer AimBarMeshRenderer;
    public override void OnTurnStart()
    {
        Debug.Log("血肉发射！");
        Destroy(BloodSoup_AimBar);
        //在玩家位置所在棋格生成攻击特效
        Instantiate(Resources.Load<GameObject>("Prefabs/Particle/EnemyEffect/Atlas1/FleshFireEffect_BloodSoup"), ChessboardManager.instance.cellStates[playerPrePos.x, playerPrePos.y].transform.position, Quaternion.identity);
        //对playerPrePos为中心3*3的区域内的玩家单位进行伤害12, 并将对应棋格cellCondition设置为BloodPool_Shallow
        for (int i = playerPrePos.x - 1; i <= playerPrePos.x + 1; i++)
        {
            for (int j = playerPrePos.y - 1; j <= playerPrePos.y + 1; j++)
            {
                if (i >= 0 && i < ChessboardManager.instance.cellStates.GetLength(0) && j >= 0 && j < ChessboardManager.instance.cellStates.GetLength(1))
                {
                    if (ChessboardManager.instance.cellStates[i, j].state == Cell.StateType.Occupied && ChessboardManager.instance.cellStates[i, j].occupant.tag == "Player")
                    {
                        ChessboardManager.instance.cellStates[i, j].occupant.GetComponent<PlayerController>().TakeDamage(12, BloodSoup.instance);
                    }
                    if (!((ChessboardManager.instance.cellStates[i, j].property is BloodPool) && (ChessboardManager.instance.cellStates[i, j].property as BloodPool).bloodPoolDepth == BloodPool.BloodPoolDepth.Deep))//如果不是深水，就设置为浅水
                    {
                        ChessboardManager.instance.ChangeProperty(new Vector2Int(i, j), "BloodPool");
                        BloodPool bloodPool = ChessboardManager.instance.cellStates[i, j].property as BloodPool;
                        bloodPool.SetBloodPool(BloodPool.BloodPoolDepth.Shallow);
                    }
                }
            }
        }
        BuffManager.instance.DeleteBuff(buffName, chessBase);
    }
    public override void OnAdd()
    {
        playerPrePos = PlayerController.instance.location;
        //在玩家位置所在棋格生成一个目标指示器
        BloodSoup_AimBar = Instantiate(Resources.Load<GameObject>("Prefabs/BloodSoup_AimBar"));
        AimBarMeshRenderer = BloodSoup_AimBar.GetComponent<MeshRenderer>();
        var position = ChessboardManager.instance.cellStates[playerPrePos.x, playerPrePos.y].transform.position;
        position.z = BloodSoup_AimBar.transform.position.z; // 保持原始的 z 值
        BloodSoup_AimBar.transform.position = position;
        BloodSoup_AimBar.transform.SetParent(ChessboardManager.instance.cellStates[playerPrePos.x, playerPrePos.y].transform, true);
    }

    void Update()
    {
        //随时间调整目标指示器的透明度造成缓慢而连续的闪烁效果
        float speed = 0.5f; // 调整闪烁速度
        float alpha = Mathf.PingPong(Time.time * speed, 0.5f) + 0.4f; // 透明度在 0.5 到 1 之间变化
        AimBarMeshRenderer.material.color = new Color(127f/255f, 0, 0, alpha);
    }
}
