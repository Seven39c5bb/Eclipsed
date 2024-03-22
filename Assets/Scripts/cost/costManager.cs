using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class costManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static costManager instance;
    public int curCost;
    public int maxCost;
    public int oriCost = 4;
    //获取费用槽字体
    public TextMeshProUGUI textMeshPro;
    private void Awake()
    {
        instance = this;
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
