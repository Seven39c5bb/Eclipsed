using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_win : FightUnit
{
    public override void Init()
    {
        //先播放胜利动画
        UIManager.Instance.ShowTip("Win!", Color.yellow, delegate ()
        {
            //从预制件中实例化WinPanel
            GameObject winPanel = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/WinPanel"), GameObject.Find("FightUI").transform);
            winPanel.transform.SetParent(GameObject.Find("FightUI").transform);

            //更新存档玩家血量
            SaveManager.instance.jsonData.playerData.HP = PlayerController.instance.HP;
            SaveManager.instance.Save();
        });
    }
    public override void OnUpdate()
    {

    }
}
