using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform canvasTf;
    public List<UIBase> uiList;//存储加载的界面的集合
    private void Awake()
    {
        Instance = this;
        canvasTf = GameObject.Find("Canvas").transform;

        //DontDestroyOnLoad(this.gameObject);

        uiList = new List<UIBase>();
    }
    void Start()
    {
        // width 和 height 为分辨率的宽和高
        SetResolution();
    }
    void SetResolution() 
    {
        float heightScal = 9.0f;
        float widthScal = 16.0f;
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        int width = Screen.width;
        int height = Screen.height;
        if (((widthScal * height) / heightScal) > screenWidth)
        {
            int h = (int)((heightScal * screenWidth) / widthScal);
            int w = (int)((widthScal * h) / heightScal);
            Screen.SetResolution(w, h, true);
        }
        else
        {
            int w = (int)((widthScal * screenHeight) / heightScal);
            int h = (int)((heightScal * screenWidth) / widthScal);
            Screen.SetResolution(w, h, true);
        }
    }
    //显示
    public UIBase ShowUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui == null)
        {
            //集合中没有，需要从Resources文件夹中加载
            GameObject obj = Instantiate(Resources.Load("Prefabs/UI/" + uiName), canvasTf) as GameObject;

            //改名字
            obj.name = uiName;

            //添加需要的脚本
            ui.AddComponent<T>();

            //添加到集合进行储存
            uiList.Add(ui);
        }
        else
        {
            ui.Show();
        }
        return ui;
    }
    //隐藏UI
    public void HideUI(string UIName)
    {
        UIBase ui=Find(UIName);
        if (ui != null)
        {
            ui.Hide();
        }
    }
    //关闭某个界面
    public void CloseUI(string UIName)
    {
        UIBase ui=Find(UIName); 
        if (ui != null)
        {
            uiList.Remove(ui);
            Destroy(ui.gameObject);
        }
    }
    //关闭所有UI
    public void CloseAll()
    {
        foreach(UIBase ui in uiList)
        {
            Destroy(ui.gameObject);
        }
        uiList.Clear();
    }
    //获取对应名字的UIBase
    public UIBase Find(string uiName)
    {
        foreach (UIBase ui in uiList)
        {
            if (ui.name == uiName) return ui;
        }
        return null;
    }
    //跳转场景
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //获得某个ui上的某个脚本
    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if(ui != null)
        {
            return ui.GetComponent<T>();
        }
        return null;
    }
    //提示轮到玩家回合
    public void ShowTip(string msg,Color color,System.Action callback=null)
    {
        GameObject obj=Instantiate(Resources.Load("Prefabs/UI/PlayerTurnTip"),canvasTf) as GameObject;
        TextMeshProUGUI text=GameObject.Find("tipMsg").GetComponent<TextMeshProUGUI>();
        Debug.Log(text.text);
        text.color = color;
        text.text = msg;
        Tween scale1 = obj.transform.DOScale(1, 0.2f);
        Tween scale2 = obj.transform.DOScale(0, 0.2f);
        DG.Tweening.Sequence seq = DOTween.Sequence();
        seq.Append(scale1);
        seq.AppendInterval(0.5f);
        seq.Append(scale2);
        seq.AppendCallback(delegate ()
        {
            if (callback != null)
            {
                callback();
            }
        });
        MonoBehaviour.Destroy(obj, 1f);
    }
}
