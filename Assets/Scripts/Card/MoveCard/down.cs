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
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private new void Update()
    {
        cost = (int)slider.value;
    }
    public override void CardFunc()
    {
        playerTransform.DOMove(new Vector3(0, playerTransform.position.y - cost, 0), 0.5f);
        costManager.instance.curCost -= cost;
    }
}
