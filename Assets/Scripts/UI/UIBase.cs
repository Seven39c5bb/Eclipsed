using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    //注册事件
    public UIEventTrigger Register(string name)
    {
        return UIEventTrigger.Get(GameObject.Find(name));
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    //关闭界面
    public virtual void Close()
    {
        UIManager.Instance.CloseUI(gameObject.name);
    }
}
