using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public  enum StateType
    {
        Empty,
        Wall,
        Occupied
    }

    public StateType state = StateType.Empty;
    public GameObject occupant = null;
    

    SpriteRenderer spriteRenderer;
    Color originColor;
    private void Awake()
    {
        if (state == StateType.Wall)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //设置为黑色
            spriteRenderer.color = Color.gray;
        }
        //记录当前颜色
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    private void OnMouseEnter()
    {
        // 将颜色设置为红色
        spriteRenderer.color = new Color(1, 0, 0, 0.05f);
    }
    private void OnMouseExit()
    {
        // 将颜色设置为原始颜色
        spriteRenderer.color = originColor;
    }
}
