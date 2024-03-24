using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class up : Card
{
    public Transform playerTransform;
    public Slider slider;
    private void Awake()
    {
        slider = this.GetComponentInChildren<Slider>();
    }
    private new void Start()
    { 
        base.Start();
    }
    private new void Update()
    {
        cost = (int)slider.value;
    }
    public override void CardFunc()
    {
        playerTransform.DOMove(new Vector3(0, playerTransform.position.y + cost, 0), 0.5f);
        costManager.instance.curCost -= cost;
    }
}
