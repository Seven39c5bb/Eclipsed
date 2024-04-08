using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //��������
    public Sprite salemanSprite;
    //�̵���Ʒ�б�
    public List<ShopItem> shopItems;
    //����
    public static ShopManager Shop_instance;
    public static ShopManager instance
    {
        get
        {
            if(Shop_instance== null)
            {
                Shop_instance=GameObject.FindObjectOfType<ShopManager>();
            }
            return Shop_instance;
        }
    }
    public List<Transform> slotTR;//�ܹ���10����λ
    //���⿨�ƹ������
    public int specialCardBuyTimes;
    private void Awake()
    {
        Shop_instance = this;
    }
    //��ʼ���̵�
    public void InitShop()
    {
        //��ʼ���̵���Ʒ
        shopItems = new List<ShopItem>();
        /*
         * ���ϵ���Ϊ4����ʽ�ƣ��м�3����ͨ�ƣ�3��ϡ���ƣ��ײ�Ϊɾ�����ƺͻظ�
         */

    }
    //
    //���ص�ͼ
    public void BackToAltas()
    {
        //����json�ļ�
        SaveManager.instance.Save();
        switch (SaveManager.instance.jsonData.mapData.backAtlasID)
        {
            case MapManager.AtlasID.Atlas_1:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_1");
                break;
            case MapManager.AtlasID.Atlas_2:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_2");
                break;
            case MapManager.AtlasID.Atlas_3:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_3");
                break;
            case MapManager.AtlasID.Atlas_4:
                SaveManager.instance.isBackFromNodeScene = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Atlas_4");
                break;
        }

    }
}
