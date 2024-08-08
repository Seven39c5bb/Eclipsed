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
    //是否被鼠标选中
    public bool isSelected = false;
    //该棋格上的属性
    public CellProperty property;


    #region 血池效果
    public enum CellCondition
    {
        Normal,
        BloodPool_Shallow,
        BloodPool_Deep
    }
    public CellCondition cellCondition = CellCondition.Normal;
    public int sustainTurn = 0;
    public void OnPlayerTurnBegin()//血池效果
    {
        switch (cellCondition)
        {
            case CellCondition.BloodPool_Shallow:
                if (state == StateType.Occupied && occupant.tag == "Player")
                {
                    BloodSoup.Instance.OnBloodPool(9);
                }
                break;
            case CellCondition.BloodPool_Deep:
                if (state == StateType.Occupied && occupant.tag == "Player")
                {
                    BloodSoup.Instance.OnBloodPool(15);
                }
                break;
        }
    }
    private GameObject bloodPool;//血池图像‘
    public void SetBloodPool(CellCondition condition)//设置血池
    {
        cellCondition = condition;
        switch (cellCondition)
        {
            case CellCondition.BloodPool_Shallow:
                if (cellCondition != CellCondition.BloodPool_Shallow)
                {
                    Destroy(bloodPool);
                    sustainTurn = 2;
                    //从预制件中实例化血池
                    bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Shallow"), transform);
                }
                else
                {
                    Destroy(bloodPool);
                    sustainTurn = 2;
                    //从预制件中实例化血池
                    bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Shallow"), transform);
                }
                break;
            case CellCondition.BloodPool_Deep:
                if (cellCondition != CellCondition.BloodPool_Deep)
                {
                    Destroy(bloodPool);
                    sustainTurn = 2;
                    //从预制件中实例化血池
                    bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Deep"), transform);
                }
                else
                {
                    Destroy(bloodPool);
                    sustainTurn = 2;
                    //从预制件中实例化血池
                    bloodPool = Instantiate(Resources.Load<GameObject>("Prefabs/BloodPool_Deep"), transform);
                }
                break;
            case CellCondition.Normal:
                Destroy(bloodPool);
                break;
        }
    }
    #endregion


    SpriteRenderer spriteRenderer;
    public Color originColor;
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
        isSelected = true;
        ChessboardManager.instance.curCell = this;
        // 将颜色设置为黄色
        if (state != StateType.Wall)
        {
            originColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1, 1, 0, 1);
        }
    }
    private void OnMouseExit()
    {
        ChessboardManager.instance.curCell = null;
        isSelected = false;
        if (state != StateType.Wall)
        {
            // 将颜色设置为原始颜色
            spriteRenderer.color = originColor;
        }
    }

    public virtual void OnPlayerEnter()
    {

    }
}
