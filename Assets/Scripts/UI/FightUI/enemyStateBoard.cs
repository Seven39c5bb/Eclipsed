using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class enemyStateBoard : MonoBehaviour, IPointerClickHandler
{
    //public string chessName;
    public TextMeshProUGUI chessHealth;
    public string chessBarrier;
    public string description;
    public UnityEngine.UI.Image HeadImg;
    public EnemyBase thisEnemy;

    public GameObject detailedPanel;
    public TextMeshProUGUI chessName;
    public TextMeshProUGUI detailedState;
    public TextMeshProUGUI CardDescription;
    public UnityEngine.UI.Image healthImage;
    public GameObject turnTip;
    private TextMeshProUGUI HealthText;
    private TextMeshProUGUI BarrierText;
    private TextMeshProUGUI MobilityText;
    private TextMeshProUGUI MoveModeText;
    private TextMeshProUGUI MeleeAttackText;

    public GameObject EntryExplanation;
    public GameObject buffBlockPrefab;
    private List<GameObject> OutsideBuffs = new List<GameObject>();
    private int preBuffListCount = 0;

    private void Start()
    {
        detailedPanel = GameObject.Find("detailedStatePanel");
        chessName=GameObject.Find("chessName").GetComponent<TextMeshProUGUI>();
        CardDescription = GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>();
        detailedState = GameObject.Find("detailedState").GetComponent<TextMeshProUGUI>();

        //从detailedPanel中获取组件
        HealthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        BarrierText = GameObject.Find("BarrierText").GetComponent<TextMeshProUGUI>();
        MobilityText = GameObject.Find("MobilityText").GetComponent<TextMeshProUGUI>();
        MoveModeText = GameObject.Find("MoveModeText").GetComponent<TextMeshProUGUI>();
        MeleeAttackText = GameObject.Find("MeleeAttackText").GetComponent<TextMeshProUGUI>();

        //加载buffBlock预制体
        buffBlockPrefab = Resources.Load<GameObject>("Prefabs/BuffBlock");

        //获取加载头像
        HeadImg = transform.Find("HeadMask/HeadImage").GetComponent<UnityEngine.UI.Image>();
        Debug.Log("Pictures/UI/HeadImage/" + thisEnemy.GetType().Name + "HeadImage");
        Sprite sprite = Resources.Load<Sprite>("Pictures/UI/HeadImage/" + thisEnemy.GetType().Name + "HeadImage");
        HeadImg.sprite = sprite;
    }
    private void Update()
    {
        //显示轮次
        if(thisEnemy.isActed) { turnTip.GetComponent<UnityEngine.UI.Image>().DOColor(Color.white, 0.2f);  }
        if (!thisEnemy.isActed && turnTip.GetComponent<UnityEngine.UI.Image>().color == Color.white) { turnTip.GetComponent<UnityEngine.UI.Image>().DOColor(Color.black, 0.2f); }
        //血量变化
        chessHealth.text = thisEnemy.HP.ToString();
        if(healthImage.fillAmount != (float)thisEnemy.HP / thisEnemy.MaxHp) { healthImage.DOFillAmount((float)thisEnemy.HP / thisEnemy.MaxHp, 0.5f); }

        if(isClicked && thisEnemy != null)
        {
            chessName.text = thisEnemy.chessName;
            HealthText.text = "生命值: " + thisEnemy.HP;
            BarrierText.text = "护盾值: " + thisEnemy.Barrier;
            MobilityText.text = "行动力: " + thisEnemy.mobility;
            MoveModeText.text = "行为模式: " + thisEnemy.moveMode;
            MeleeAttackText.text = "近战伤害: " + thisEnemy.MeleeAttackPower;
            CardDescription.text = thisEnemy.chessDiscrption;

            if (preBuffListCount != thisEnemy.buffList.Count)
            {
                foreach (var buffBlock in stateBoard.instance.buffBlockList)
                {
                    Destroy(buffBlock);
                }
                stateBoard.instance.buffBlockList.Clear();
                foreach (var buff in thisEnemy.buffList)
                {
                    GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                    buffBlock.GetComponent<buffBoard>().isDetailed = true;
                    buffBlock.GetComponent<buffBoard>().buff = buff;
                    stateBoard.instance.buffBlockList.Add(buffBlock);
                }
            }
        }

        if (preBuffListCount != thisEnemy.buffList.Count)
        {
            foreach (var buffBlock in OutsideBuffs)
            {
                Destroy(buffBlock);
            }
            OutsideBuffs.Clear();
            foreach (var buff in thisEnemy.buffList)
            {
                GameObject buffBlock = Instantiate(buffBlockPrefab, transform.Find("OutsideBuffs").transform) as GameObject;
                buffBlock.GetComponent<buffBoard>().isDetailed = false;//外部buff不显示详细信息
                buffBlock.GetComponent<buffBoard>().buff = buff;
                OutsideBuffs.Add(buffBlock);
            }
            preBuffListCount = thisEnemy.buffList.Count;
        }
        
    }

    public bool isClicked = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(thisEnemy != null)
        {
            if (detailedPanel.GetComponent<CanvasGroup>().alpha == 0) 
            {
                foreach(var buffBlock in stateBoard.instance.buffBlockList)
                {
                    Destroy(buffBlock);
                }
                stateBoard.instance.buffBlockList.Clear();
                detailedPanel.GetComponent<CanvasGroup>().alpha = 1;
                detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                foreach (var buff in thisEnemy.buffList)
                {
                    GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                    buffBlock.GetComponent<buffBoard>().buff = buff;
                    stateBoard.instance.buffBlockList.Add(buffBlock);
                }
                preBuffListCount = thisEnemy.buffList.Count;
                isClicked = true;
            } 
            else if(isClicked == true)
            {
                detailedPanel.GetComponent<CanvasGroup>().alpha = 0;
                detailedPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                isClicked = false;
            }
            else//此面板未被点击，但具体信息面板被激活，说明其他面板被点击，将其他面板的点击状态设为false
            {
                enemyStateBoard[] enemyStateBoards = GameObject.FindObjectsOfType<enemyStateBoard>();
                foreach (enemyStateBoard board in enemyStateBoards)
                {
                    board.isClicked = false;
                }
                foreach (var buffBlock in stateBoard.instance.buffBlockList)
                {
                    Destroy(buffBlock);
                }
                stateBoard.instance.buffBlockList.Clear();
                stateBoard.instance.isClicked = false;
                foreach(var buff in thisEnemy.buffList)
                {
                    GameObject buffBlock = Instantiate(buffBlockPrefab, GameObject.Find("BuffGrid").transform) as GameObject;
                    buffBlock.GetComponent<buffBoard>().buff = buff;
                    stateBoard.instance.buffBlockList.Add(buffBlock);
                }
                preBuffListCount = thisEnemy.buffList.Count;
                isClicked = true;
            }
            detailedPanel.GetComponent<CanvasGroup>().DOFade(detailedPanel.GetComponent<CanvasGroup>().alpha, 0.2f);
        }
    }
}
