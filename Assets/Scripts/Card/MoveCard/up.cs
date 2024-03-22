using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class up : Card
{
    private new void Start()
    {
        base.Start();
    }
    private new void Update()
    {
        
    }
    public void MoveUp()
    {
        Slider slider = this.GetComponentInChildren<Slider>();
        cost= (int)slider.value;
        PlayerController.instance.Move(new Vector2Int(0, -cost));
        costManager.instance.curCost -= cost;
    }
}
