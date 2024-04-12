using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBase
{
    //选项描述
    public string optionDescription;
    //选项功能
    public string optionFunc;
    //选项数据
    public string optionData;
    //选项结果
    public string optionResult;
    public EventBase(string optionDescription,string optionResult, string optionFunc, string optionData)
    {
        this.optionDescription = optionDescription;
        this.optionResult = optionResult;
        this.optionFunc = optionFunc;
        this.optionData = optionData;
    }
}
