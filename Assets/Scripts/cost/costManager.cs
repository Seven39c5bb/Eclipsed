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
    //��ȡ���ò�����
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
        textMeshPro.text=curCost.ToString()+'/'+maxCost.ToString();//ʵʱ��ʾʣ����ú�max����
    }
}
