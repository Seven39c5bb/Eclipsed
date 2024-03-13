using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    //ע���¼�
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
    //�رս���
    public virtual void Close()
    {
        UIManager.Instance.CloseUI(gameObject.name);
    }
}
