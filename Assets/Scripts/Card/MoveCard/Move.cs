using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Move : Card
{
    public enum state
    {
        none,up,down,left,right
    }
    public state moveState;//行动方式
    //获取上下左右对应四个按钮
    public Button upBtn,downBtn,leftBtn,rightBtn;
    public TextMeshProUGUI nameTxt;//卡牌名字
    public Transform playerTransform;//玩家位置
    public Slider slider;//滑条
    private new void Start()
    {
        base.Start();
        InitBtns();
    }

    private void InitBtns()
    {
        upBtn.onClick.AddListener(() => { moveState = state.up; });
        downBtn.onClick.AddListener(() => { moveState = state.down; });
        leftBtn.onClick.AddListener(() => { moveState = state.left; });
        rightBtn.onClick.AddListener(() => { moveState = state.right; });
    }

    private new void Update()
    {
        cost = (int)slider.value;
        costText.text = cost.ToString();
        switch (moveState)
        {
            case state.none:
                nameTxt.text = "移动";
                break;
            case state.up:
                nameTxt.text = "上行";
                break;
            case state.down:
                nameTxt.text = "下行";
                break;
            case state.left:
                nameTxt.text = "左行";
                break;
            case state.right:
                nameTxt.text = "右行";
                break;
        }
        //discriptionText.text = "向"+"移动**" + (cost + 1).ToString() + "**格\r\n（滑动滑块调整）";
    }
    public override void CardFunc()
    {
        switch (moveState)
        {
            case state.none:
                this.isUsed = false;
                return;
            case state.up:
                //PlayerController.instance.Move(new Vector2Int(0, -cost - 1));
                PlayerController.instance.StartCoroutine(PlayerController.instance.Move(new Vector2Int(0, -cost - 1)));
                costManager.instance.curCost -= cost;
                break;
            case state.down:
                //PlayerController.instance.Move(new Vector2Int(0, cost + 1));
                PlayerController.instance.StartCoroutine(PlayerController.instance.Move(new Vector2Int(0, cost + 1)));
                costManager.instance.curCost -= cost;
                break;
            case state.left:
                //PlayerController.instance.Move(new Vector2Int(-cost - 1, 0));
                PlayerController.instance.StartCoroutine(PlayerController.instance.Move(new Vector2Int(-cost - 1, 0)));
                costManager.instance.curCost -= cost;
                break;
            case state.right:
                //PlayerController.instance.Move(new Vector2Int(cost + 1, 0));
                PlayerController.instance.StartCoroutine(PlayerController.instance.Move(new Vector2Int(cost + 1, 0)));
                costManager.instance.curCost -= cost;
                break;
        }
    }
}
