using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //��������
    public Sprite salemanSprite;
    //�̵���Ʒ�б�
    public List<ShopItem> shopItems;
    public static ShopManager instance;
    public List<Transform> slotTR;//�ܹ���10����λ
    //���⿨�ƹ������
    public int specialCardBuyTimes;
    private void Awake()
    {
        instance = this;
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
}
