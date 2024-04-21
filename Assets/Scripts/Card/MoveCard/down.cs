using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class down : Card
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
        PlayerController.instance.Move(new Vector2Int(0, cost+1));
        costManager.instance.curCost -= cost;
    }
}
