using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LossContinueUI : UIBase
{
    void Awake()
    {
        Register("LossBackButton").onClick = OnClickBackMenu;
        this.gameObject.transform.SetParent(GameObject.Find("FightUI").transform);
    }

    void OnClickBackMenu(GameObject obj, PointerEventData eventData)
    {
        SceneManager.LoadScene("Start");
    }
}
