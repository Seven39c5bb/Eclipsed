using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameStartUI : UIBase
{
    private void Start()
    {
        Register("Start").onClick = LoadSceneAtlas_1;
        Register("Continue").onClick = LoadSceneContinue;
        Register("Quit").onClick = OuitGame;
        AudioManager.PlayBGM("Cover");
    }
    void LoadSceneAtlas_1(GameObject obj, PointerEventData eventData)
    {
        SaveManager.instance.DeleteSave();//删档重来
        SaveManager.instance.InitJsonData();
        UIManager.Instance.LoadScene("Atlas_1");
    }
    void LoadSceneContinue(GameObject obj, PointerEventData eventData)
    {
        SaveManager.instance.Load();
        UIManager.Instance.LoadScene("Atlas_" + MapManager.AtlasIDToInt(SaveManager.instance.jsonData.mapData.currAtlasID));
    }
    void OuitGame(GameObject obj, PointerEventData eventData)
    {
        Application.Quit();
    }
    void About()
    {
        
    }

}
