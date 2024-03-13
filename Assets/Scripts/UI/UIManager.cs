using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform canvasTf;//???
    public List<UIBase> uiList;//�洢���صĽ���ļ���
    private void Awake()
    {
        Instance = this;
        canvasTf = GameObject.Find("Canvas").transform;
        uiList = new List<UIBase>();
    }
    //��ʾ
    public UIBase ShowUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui == null)
        {
            //������û�У���Ҫ��Resources�ļ����м���
            GameObject obj = Instantiate(Resources.Load("UI/Prefabs" + uiName), canvasTf) as GameObject;

            //������
            obj.name = uiName;

            //�����Ҫ�Ľű�
            ui.AddComponent<T>();

            //��ӵ����Ͻ��д���
            uiList.Add(ui);
        }
        else
        {
            ui.Show();
        }
        return ui;
    }
    //����UI
    public void HideUI(string UIName)
    {
        UIBase ui=Find(UIName);
        if (ui != null)
        {
            ui.Hide();
        }
    }
    //�ر�ĳ������
    public void CloseUI(string UIName)
    {
        UIBase ui=Find(UIName); 
        if (ui != null)
        {
            uiList.Remove(ui);
            Destroy(ui.gameObject);
        }
    }
    //�ر�����UI
    public void CloseAll()
    {
        foreach(UIBase ui in uiList)
        {
            Destroy(ui.gameObject);
        }
        uiList.Clear();
    }
    //��ȡ��Ӧ���ֵ�UIBase
    public UIBase Find(string uiName)
    {
        foreach (UIBase ui in uiList)
        {
            if (ui.name == uiName) return ui;
        }
        return null;
    }
}
