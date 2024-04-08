using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    }
}
