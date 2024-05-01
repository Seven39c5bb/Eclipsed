using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class WitchDialogue : UIBase
{
    List<string> originDialogues = new List<string>();
    List<string> dialogues = new List<string>();
    CanvasGroup DialoguePanel;
    void Awake()
    {
        Register("Witch").onClick = OnClickWitch;
        DialoguePanel = GameObject.Find("DialoguePanel").GetComponent<CanvasGroup>();

        // 初始化对话文本
        originDialogues.Add("......");
        originDialogues.Add("谨遵母亲大人的教诲。");
        originDialogues.Add("这座城市已被色彩污染。");
        originDialogues.Add("......祂在邀请我。");
        originDialogues.Add("我能看到、听到、触摸到这种色彩......不，不行。");
        originDialogues.Add("城市的居民被祂的力量扭曲而变作怪物，真是可悲。");
        if(SaveManager.instance.jsonData.playerData.HP >= 55) originDialogues.Add("我的体力充足，不需要花费时间修整。");
        if(SaveManager.instance.jsonData.playerData.HP < 55 && SaveManager.instance.jsonData.playerData.HP >= 30) originDialogues.Add("或许我该稍作休整。");
        if(SaveManager.instance.jsonData.playerData.HP < 30) originDialogues.Add("我需要......休息......");


        dialogues.AddRange(originDialogues);

        int index = Random.Range(0, dialogues.Count); // 获取随机索引
        currText = dialogues[index]; // 获取当前文本
        dialogues.RemoveAt(index); // 将当前文本从列表中移除
        // 使用DoTween动画，将对话框的透明度从0变为1
        DialoguePanel.DOFade(1, 0.5f);
        textLabel = GameObject.Find("WitchDialogue").GetComponent<TextMeshProUGUI>();
        StartCoroutine(SetTextUI());
        timer = 0;
    }

    float timer = 0;
    float timeLimit = 5; // 设置时间限制为5秒

    void Update()
    {
        // 一段时间后隐藏文本框
        // 如果文本框是可见的
        if (DialoguePanel.alpha > 0)
        {
            // 增加计时器
            timer += Time.deltaTime;

            // 如果计时器超过了时间限制
            if (timer > timeLimit)
            {
                // 隐藏文本框
                DialoguePanel.DOFade(0, 0.5f);
                // 重置计时器
                timer = 0;
            }
        }
    }

    bool isAnimating = false;
    int dialoguesCounter = 0;
    void OnClickWitch(GameObject obj, PointerEventData eventData)
    {

        if (isAnimating || !textFinished) return;

        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        Vector3 originalScale = rectTransform.localScale;

        // 将大小缩小一点
        Vector3 targetScale = originalScale * 0.95f;

        // 创建一个序列，以便我们可以将多个动画链接在一起
        Sequence sequence = DOTween.Sequence();

        // 将大小动画添加到序列中
        sequence.Append(rectTransform.DOScale(targetScale, 0.2f));

        // 然后，将大小恢复到原来的值
        sequence.Append(rectTransform.DOScale(originalScale, 0.2f));

        // 在动画开始时，设置isAnimating为true
        sequence.OnStart(() => {
            isAnimating = true;
            if(dialogues.Count == 0)
            {
                dialogues.AddRange(originDialogues);
            }

            int index = Random.Range(0, dialogues.Count); // 获取随机索引
            currText = dialogues[index]; // 获取当前文本
            dialogues.RemoveAt(index); // 将当前文本从列表中移除

            dialoguesCounter++;//小彩蛋计数器
            if(dialoguesCounter >= 20)
            {
                currText = "......你很无聊吗？";
            }

            // 使用DoTween动画，将对话框的透明度从0变为1
            DialoguePanel.DOFade(1, 0.5f);
            textLabel = GameObject.Find("WitchDialogue").GetComponent<TextMeshProUGUI>();
            StartCoroutine(SetTextUI());
            timer = 0;
        });

        // 在动画结束时，设置isAnimating为false
        sequence.OnComplete(() => {
            isAnimating = false;
        });

    }

    bool textFinished = true;
    TextMeshProUGUI textLabel;
    string currText = "";
    public float textSpeed = 0.1f;
    IEnumerator SetTextUI()//用于间时展示文本输出的协程
    {
        textFinished = false;
        textLabel.text = "";

        for(int i = 0; i < currText.Length; i++)
        {
            textLabel.text += currText[i];

            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true;//防止玩家切换文本过快出错
    }
}
