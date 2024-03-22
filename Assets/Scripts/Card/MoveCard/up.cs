using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class up : Card
{
    public Transform playerTransform;
    private new void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private new void Update()
    {
        
    }
    public void MoveUp()
    {
        Slider slider = this.GetComponentInChildren<Slider>();
        cost= (int)slider.value;
        playerTransform.DOMove(new Vector3(0, playerTransform.position.y+cost, 0), 0.5f);
        costManager.instance.curCost -= cost;
    }
}
