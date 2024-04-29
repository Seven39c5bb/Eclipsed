using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePanel : MonoBehaviour
{
    public Step[] steps;
    public int currentStep;
    public static GuidePanel _instance;
    public static GuidePanel instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GuidePanel>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        //初始化所有的引导步骤
        steps = new Step[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            steps[i] = transform.GetChild(i).GetComponent<Step>();
        }
        ExecuteStep(0);
    }
    //执行某一个步骤
    public void ExecuteStep(int index)
    {
        this.gameObject.SetActive(true);
        //隐藏所有的步骤
        HideAllSteps();

        currentStep = index;
        if (index < 0 || index >= steps.Length)
        {
            this.gameObject.SetActive(false);
            return;
        }
        steps[index].gameObject.SetActive(true);
        steps[index].Execute();
    }
    //执行下一个步骤
    public void ExecuteNextStep(int eventIndex)
    {
        if (eventIndex == steps[currentStep].eventIndex)
        {
            currentStep++;
            ExecuteStep(currentStep);
        }
        
    }
    //隐藏所有的步骤
    public void HideAllSteps()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].gameObject.SetActive(false);
        }
    }
}
