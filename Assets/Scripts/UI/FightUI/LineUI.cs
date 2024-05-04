using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineUI : MonoBehaviour
{
    public GameObject arrowHead;
    public GameObject arrowNode;
    public int arrowNum;
    public float scaleFactor;

    private RectTransform origin;//P0的位置
    private List<RectTransform> arrowNodes=new List<RectTransform>();//箭头节点的位置
    private List<Vector2> controlPoints=new List<Vector2>();//控制点的位置
    private readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };//控制点的位置系数
    private void Awake()
    {
        this.origin=this.GetComponent<RectTransform>();
        //生成箭头和箭头结点
        for(int i = 0; i < arrowNum; i++)
        {
            this.arrowNodes.Add(Instantiate(arrowNode, this.transform).GetComponent<RectTransform>());
        }
        this.arrowNodes.Add(Instantiate(arrowHead, this.transform).GetComponent<RectTransform>());

        //隐藏所有箭头
        this.arrowNodes.ForEach(node =>
        {
            node.GetComponent<RectTransform>().position=new Vector2(-1000, -1000);
        });

        //初始化控制点列表
        for(int i = 0; i < 4; i++)
        {
            this.controlPoints.Add(new Vector2(0, 0));
        }
    }
    private void Update()
    {
        //计算四个控制点的位置
        this.controlPoints[0] = this.origin.position-new Vector3(-500f,600f,0);

        this.controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];

        for(int i=0;i<this.arrowNodes.Count;i++)
        {
            float t=Mathf.Log(1f*i/(this.arrowNodes.Count-1)+1f,2f);

            //计算贝塞尔曲线上的点
            this.arrowNodes[i].position=
                Mathf.Pow(1 - t, 3) * this.controlPoints[0]+
                3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1]+
                3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2]+
                Mathf.Pow(t, 3) * this.controlPoints[3];

            //计算箭头的旋转角度
            if (i > 0)
            {
                var euler=new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i-1].position));
                this.arrowNodes[i].rotation=Quaternion.Euler(euler);
            }
            //计算箭头的缩放
            var scale = this.scaleFactor*(1f-0.03f*(this.arrowNodes.Count-i-1));
            this.arrowNodes[i].localScale=new Vector3(scale, scale, 1f);
        }
        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
}
