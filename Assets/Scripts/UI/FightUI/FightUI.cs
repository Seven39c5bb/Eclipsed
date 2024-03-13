using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightUI : UIBase
{
    private void Awake()
    {
        Register("endTurnButton").onClick = onClickEndTurn;
    }
    private void onClickEndTurn(GameObject obj,PointerEventData eventData)
    {
        //’Ω∂∑≥ı ºªØ
        FightManager.instance.ChangeType(FightType.Init);
        Debug.Log("hide the button endturn");     
    }
}
