using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RectGuide : MonoBehaviour,ICanvasRaycastFilter
{
    public static RectGuide _instance;
    public static RectGuide instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RectGuide>();
            }
            return _instance;
        }
    }

    public Material material;//材质
    public Vector3[] targetCorners = new Vector3[4];//目标的四个角
    public Vector3 center; //中心点
    public float width;     //宽度
    public float height;// 高度

    public RectTransform target;//需要高亮的目标

    private void Awake()
    {
        
    }
    private void Start()
    {
        material=this.transform.GetComponent<Image>().material;
        if(material == null)
        {
            throw new System.Exception("请给Image添加材质");
        }
    }
    private void Update()
    {
        Rect_Guide(target);
    }

    public void Rect_Guide(RectTransform target)
    {
        this.target = target;

        //获取目标的四个角
        target.GetWorldCorners(targetCorners);
        for(int i = 0; i < 4; i++)
        {
            //把世界坐标转换为屏幕坐标
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(targetCorners[i]);
            //把屏幕坐标转换为局部坐标
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").GetComponent<RectTransform>(), 
                screenPoint, Camera.main, out localPoint);
            targetCorners[i] = localPoint;
        }
        center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
        center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;
        material.SetVector("_Center", center);


        //计算宽度和高度
        width = (targetCorners[3].x - targetCorners[0].x)/1.5f;
        height = (targetCorners[1].y - targetCorners[0].y)/1.5f;
        material.SetFloat("_SliderX", width);
        material.SetFloat("_SliderY", height);
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (target == null) { return true; }
        //判断点击的点是否在目标区域内
        return !RectTransformUtility.RectangleContainsScreenPoint(target, sp);
    }
}
