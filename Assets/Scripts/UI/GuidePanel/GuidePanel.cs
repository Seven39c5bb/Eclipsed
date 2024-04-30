using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class GuidePanelImg
{
    public int index;
    public Image img;
    public string imgName;
}
public class GuidePanel : MonoBehaviour
{
    public List<Sprite> guideImgs;
    public Image curGuideImg;
    private void Awake()
    {
        //展示第一张图片
        curGuideImg.sprite = guideImgs[0];
    }
    public void ShowImg(int index)
    {
        curGuideImg.sprite = guideImgs[index];
    }
    public void Close()
    {
        Destroy(gameObject);
    }
    
}
