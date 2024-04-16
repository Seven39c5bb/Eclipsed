using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinContinueButton : MonoBehaviour
{
    //胜利时点击按钮返回地图，统一在这里保存
    public static void BackToAtlas()
    {
        //更新存档玩家血量
        //SaveManager.instance.jsonData.playerData.HP = PlayerController.instance.HP;
        SaveManager.instance.Save();

        //根据当前地图ID返回对应地图
        switch(SaveManager.instance.jsonData.mapData.backAtlasID)
        {
            case MapManager.AtlasID.Atlas_1:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_1");
                break;
            case MapManager.AtlasID.Atlas_2:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_2");
                break;
            case MapManager.AtlasID.Atlas_3:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_3");
                break;
            case MapManager.AtlasID.Atlas_4:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_4");
                break;
        }
    }
}
