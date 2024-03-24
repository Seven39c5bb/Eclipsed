using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class right : Card
{
    public Transform playerTransform;
    public Slider slider;
    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
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
        PlayerController.instance.Move(new Vector2Int(cost, 0));
        costManager.instance.curCost -= cost;
    }
}