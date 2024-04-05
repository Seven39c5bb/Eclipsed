using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameStartUI : UIBase
{
    private void Awake()
    {
        Register("Start").onClick = LoadSceneCardTest;
    }
    public void LoadSceneCardTest(GameObject obj, PointerEventData eventData)
    {
        //SaveManager.instance.DeleteSave();//删档重来
        UIManager.Instance.LoadScene("Atlas_1");
    }
}
