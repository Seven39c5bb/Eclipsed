using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineUI : MonoBehaviour
{
    private void Update()
    {
        UpdateLine();
    }
    //更新线
    public void UpdateLine()
    {
        Vector3 mousePos = Input.mousePosition;
        SetEndPos(mousePos);
    }
    //设置开始位置
    public void SetStartPos(Vector2 pos)
    {
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
    }
    //设置终点位置
    public void SetEndPos(Vector2 pos)
    {
        transform.GetChild(transform.childCount-1).GetComponent<RectTransform>().anchoredPosition = pos;
        //开始点
        Vector3 startPos = transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        //终点
        Vector3 endPos = pos;
        //中点
        Vector3 midPos = Vector3.zero;
        midPos.y = (startPos.y + endPos.y) / 2;
        midPos.x = startPos.x;
        //计算开始点与终点的方向
        Vector3 dir= (endPos - startPos).normalized;
        float angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;//弧度转角度
        //设置终点角度
        transform.GetChild(transform.childCount - 1).eulerAngles = new Vector3(0, 0, angle);
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition=GetBezier(startPos,endPos,midPos,i/(float)(transform.childCount));
            if (i != transform.childCount - 1)
            {
                dir=(transform.GetChild(i + 1).GetComponent<RectTransform>().anchoredPosition - transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition).normalized;
                angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
                transform.GetChild(i).eulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
    public Vector3 GetBezier(Vector3 startPos, Vector3 endPos, Vector3 midPos, float t)
    {
        return (1.0f - t) * (1.0f - t) * startPos + 2 * t * (1.0f - t) * midPos + t * t * endPos;
    }
}
