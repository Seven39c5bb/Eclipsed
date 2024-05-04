using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class costManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static costManager cost_instance;
    public static costManager instance
    {
        get
        {
            if (cost_instance == null)
            {
                cost_instance= GameObject.FindObjectOfType<costManager>();
            }
            return cost_instance;
        }
    }
    public int curCost;
    public int maxCost;
    public int oriCost = 4;
    //获取费用槽字体
    public TextMeshProUGUI textMeshPro;
    public Image CostEye; // 新增的 CostEye 图片
    private void Awake()
    {
        cost_instance = this;
    }
    void Start()
    {
        curCost = oriCost;
        maxCost = oriCost;
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text=curCost.ToString()+'/'+maxCost.ToString();//实时显示剩余费用和max费用
        if (curCost == 0)
        {
            CostEye.DOColor(new Color(70f/255f, 90f/255f, 140f/255f, 1), 0.5f); // 当费用为0时，将 CostEye 图片的颜色调暗
        }
        else
        {
            CostEye.DOColor(new Color(1, 200f/255f, 1, 1), 0.5f); // 当费用不为0时，将 CostEye 图片的颜色调回白色
        }
    }
}
