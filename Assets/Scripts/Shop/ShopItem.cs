using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    //��Ʒ����
    public string itemName;
    //��ʲô����
    public enum buyItem { coin,fingerbone}
    public enum itemType { card,deleteCard}
    public itemType type;
    //��Ʒ�۸�
    public int price;
    //��Ʒͼ��
    public Sprite itemSprite;
    //��Ʒ����
    public string itemDescription;
    //��Ʒ�ɹ������
    public int buyTimes;
    //��������������0�������ٴι���
    //����
    void BUY_THIS()
    {
        //�����������Ʒʱ
        switch(this.type)
        {
            case itemType.card:
                //������
                //������Ŀ��Ʒ��뿨��
                break;
            case itemType.deleteCard:
                //����ɾ������
                //��ɾ�����ƵĽ���
                //ɾ������
                break;
        }
    }
}
