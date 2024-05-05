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
        startColor=this.GetComponent<Image>().color;
        //找到该物体下的costText
        if(this.transform.Find("cost") != null)
        {
            costText = this.transform.Find("cost").GetComponent<TextMeshProUGUI>();
            costText.text = cost.ToString();
        }
        //找到该物体下的discriptionText
        if(this.transform.Find("Text (TMP) (1)") != null)
        {
            discriptionText = this.transform.Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>();
            //discriptionText.text = discription;
        }
    }
    private new void Start()
    {
        base.Start();
    }
    private new void Update()
    {
        cost = (int)slider.value;
        costText.text = cost.ToString();
        discriptionText.text = "向右移动**" + (cost + 1).ToString() + "**格\r\n（滑动滑块调整）";
    }
    public override void CardFunc()
    {
        PlayerController.instance.Move(new Vector2Int(cost+1, 0));
        costManager.instance.curCost -= cost;
    }
}